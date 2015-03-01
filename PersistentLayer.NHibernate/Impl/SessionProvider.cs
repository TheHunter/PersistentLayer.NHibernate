using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// Class SessionProvider.
    /// </summary>
    [Serializable]
    public abstract class SessionProvider
        : ISessionProvider
    {
        private readonly Stack<ITransactionInfo> transactions;
        /// <summary>
        /// This is the default naming for transactions.
        /// </summary>
        private const string DefaultNaming = "anonymous";

        #region Session factory section

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionProvider"/> class.
        /// </summary>
        protected SessionProvider()
        {
            transactions = new Stack<ITransactionInfo>();
        }

        #endregion

        /// <summary>
        /// Gets the current bounded session by a higher implementation level.
        /// </summary>
        /// <returns>Returns the current binded session by a higher implementation level.</returns>
        public abstract ISession GetCurrentSession();

        /// <summary>
        /// Indicates if the root transaction is in progress.
        /// </summary>
        /// <value><c>true</c> if [in progress]; otherwise, <c>false</c>.</value>
        public bool InProgress
        {
            get { return this.transactions.Count > 0; }
        }

        /// <summary>
        /// Indicates if there's a transaction with the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Exists(string name)
        {
            if (name == null)
                return false;

            return this.transactions.Count(info => info.Name == name) > 0;
        }

        /// <summary>
        /// Begin a new transaction with a default name target.
        /// </summary>
        public virtual void BeginTransaction()
        {
            this.BeginTransaction((IsolationLevel?)null);
        }

        /// <summary>
        /// Begin a new transaction with the given name target.
        /// </summary>
        /// <param name="name">The name.</param>
        public virtual void BeginTransaction(string name)
        {
            // ReSharper disable RedundantCast
            this.BeginTransaction(name, (IsolationLevel?)null);
            // ReSharper restore RedundantCast
        }

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        /// <param name="level">The level.</param>
        public virtual void BeginTransaction(IsolationLevel? level)
        {
            int index = transactions.Count;
            this.BeginTransaction(string.Format("{0}_{1}", DefaultNaming, index), level);
        }

        /// <summary>
        /// Begin a new transaction with the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="level">The level.</param>
        /// <exception cref="PersistentLayer.Exceptions.BusinessLayerException">
        /// The transaction name cannot be null or empty;BeginTransaction
        /// or
        /// BeginTransaction
        /// or
        /// Error on beginning a new transaction.;BeginTransaction
        /// </exception>
        public virtual void BeginTransaction(string name, IsolationLevel? level)
        {
            if (name == null || name.Trim().Equals(string.Empty))
                throw new BusinessLayerException("The transaction name cannot be null or empty", "BeginTransaction");

            if (this.Exists(name))
                throw new BusinessLayerException(string.Format("The transaction name ({0}) to add is used by another point.", name), "BeginTransaction");

            int index = transactions.Count;
            ITransactionInfo info = new TransactionInfo(name, index);

            if (this.transactions.Count == 0)
            {
                try
                {
                    ISession session = this.GetCurrentSession();
                    if (level == null)
                        session.BeginTransaction();
                    else
                        session.BeginTransaction(level.Value);
                }
                catch (Exception ex)
                {
                    throw new BusinessLayerException("Error on beginning a new transaction.", "BeginTransaction", ex);
                }
            }
            this.transactions.Push(info);
        }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        /// <exception cref="PersistentLayer.Exceptions.CommitFailedException">CommitTransaction</exception>
        public virtual void CommitTransaction()
        {
            if (transactions.Count > 0)
            {
                ITransactionInfo info = transactions.Pop();
                if (transactions.Count == 0)
                {
                    ITransaction transaction = null;
                    ISession session;

                    try
                    {
                        session = this.GetCurrentSession();
                        if (session == null || !session.IsOpen)
                            return;
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    try
                    {
                        transaction = session.Transaction;
                        if (session.FlushMode == FlushMode.Never)
                        {
                            session.Flush();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (transaction != null && transaction.IsActive)
                            transaction.Rollback();

                        throw new CommitFailedException(string.Format("Error when the current session tries to commit the current transaction (name: {0}).", info.Name), "CommitTransaction", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Makes a rollback the transaction.
        /// </summary>
        public virtual void RollbackTransaction()
        {
            this.RollbackTransaction(null);
        }

        /// <summary>
        /// Makes a rollback, indicating the exception associated to the last transaction.
        /// </summary>
        /// <param name="cause">The cause.</param>
        /// <exception cref="PersistentLayer.Exceptions.RollbackFailedException">Error on calling RollbackTransaction method</exception>
        /// <exception cref="PersistentLayer.Exceptions.InnerRollBackException">An inner rollback transaction has occurred.</exception>
        public virtual void RollbackTransaction(Exception cause)
        {
            if (transactions.Count > 0)
            {
                ITransactionInfo info = transactions.Pop();
                try
                {
                    ISession session;
                    try
                    {
                        session = this.GetCurrentSession();
                        if (session == null || !session.IsOpen)
                            return;
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    
                    try
                    {
                        ITransaction transaction = session.Transaction;
                        transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        throw new RollbackFailedException("Error on calling RollbackTransaction method", cause, info, ex);
                    }

                    /*
                     It means the calling instance has opened more than one transaction, so in this case this instance
                     throws an exception in order for advising the calling instance that there were inner transaction in progress.
                     * 
                     */
                    if (transactions.Count > 0)
                        throw new InnerRollBackException("An inner rollback transaction has occurred.", cause, info);
                }
                finally
                {
                    this.transactions.Clear();
                }
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        protected virtual void Reset()
        {
            this.transactions.Clear();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            try
            {
                if (this.InProgress)
                    this.RollbackTransaction();
            }
            catch
            {
            }
            finally
            {
                this.Reset();
            }
        }
    }
}
