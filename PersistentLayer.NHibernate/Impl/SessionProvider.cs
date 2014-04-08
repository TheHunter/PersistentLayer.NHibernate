using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class SessionProvider
        : ISessionProvider
    {
        private readonly Stack<ITransactionInfo> transactions;
        /// <summary>
        /// This is the factory which creates sessions, and It's able to reference the current binded session
        /// made by CurrentSessionContext
        /// </summary>
        
        private const string DefaultNaming = "anonymous";

        #region Session factory section

        /// <summary>
        /// 
        /// </summary>
        protected SessionProvider()
        {
            transactions = new Stack<ITransactionInfo>();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract ISession GetCurrentSession();
        

        /// <summary>
        /// 
        /// </summary>
        public bool InProgress
        {
            get { return this.transactions.Count > 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            if (name == null)
                return false;

            return this.transactions.Count(info => info.Name == name) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginTransaction()
        {
            this.BeginTransaction((IsolationLevel?)null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void BeginTransaction(string name)
        {
            // ReSharper disable RedundantCast
            this.BeginTransaction(name, (IsolationLevel?)null);
            // ReSharper restore RedundantCast
        }

        /// <summary>
        /// Begin a new transaction from the current binded session with the specified IsolationLevel.
        /// </summary>
        /// <param name="level">IsolationLevel for this transaction.</param>
        /// <exception cref="BusinessLayerException"></exception>
        /// <exception cref="SessionNotBindedException">
        /// Throws an exception when there's no session binded into any CurrentSessionContext.
        /// </exception>
        public void BeginTransaction(IsolationLevel? level)
        {
            int index = transactions.Count;
            this.BeginTransaction(string.Format("{0}_{1}", DefaultNaming, index), level);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="level"></param>
        /// <exception cref="BusinessLayerException"></exception>
        public void BeginTransaction(string name, IsolationLevel? level)
        {
            if (name == null || name.Trim().Equals(string.Empty))
                throw new BusinessLayerException("The transaction name cannot be null or empty", "Exists");

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
                catch (SessionNotBindedException ex)
                {
                    throw new BusinessLayerException("Error on beginning a new transaction.", "BeginTransaction", ex);
                }
            }
            this.transactions.Push(info);
        }

        /// <summary>
        /// Commit the current transaction and flushes the associated session.
        /// </summary>
        /// <exception cref="CommitFailedException">
        /// Throws an exception when current transaction tries to commit.
        /// </exception>
        public void CommitTransaction()
        {
            if (transactions.Count > 0)
            {
                ITransactionInfo info = transactions.Pop();
                if (transactions.Count == 0)
                {
                    ITransaction transaction = null;
                    try
                    {
                        ISession session = this.GetCurrentSession();
                        transaction = session.Transaction;
                        if (session.FlushMode == FlushMode.Never)
                        {
                            session.Flush();
                        }
                        transaction.Commit();
                    }
                    catch (SessionNotBindedException)
                    {
                        /*
                         No session instance was binded... 
                         In this case It supposes that there's any actions to compute with data storage
                        */
                    }
                    catch (Exception ex)
                    {
                        // accettarsi che la chiamata a RollBack non generi un'eccezione
                        // in quel caso occorre gestirlo, eventualement risollervalo incapsulandolo in un'altra eccezione
                        if (transaction != null) transaction.Rollback();

                        throw new CommitFailedException(string.Format("Error when the current session tries to commit the current transaction (name: {0}).", info.Name), "CommitTransaction", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Makes a rollback into current transaction
        /// </summary>
        /// <exception cref="RollbackFailedException">
        /// Throws an exception when current transaction makes a rollback.
        /// </exception>
        /// <exception cref="InnerRollBackException">
        /// Throws an exception when an inner transaction makes a rollback.
        /// </exception>
        public void RollbackTransaction()
        {
            this.RollbackTransaction(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cause"></param>
        public void RollbackTransaction(Exception cause)
        {
            if (transactions.Count > 0)
            {
                ITransactionInfo info = transactions.Pop();
                try
                {
                    ISession session = this.GetCurrentSession();
                    ITransaction transaction = session.Transaction;
                    try
                    {
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
                    {
                        throw new InnerRollBackException("An inner rollback transaction has occurred.", cause, info);
                    }
                }
                catch (SessionNotBindedException)
                {
                    #region
                    // eccezione sollevata dalla chiamata a GetCurrentSession()
                    // e il motivo di questa eccezione è causato dal mancato binding della sessione corrente.
                    /*
                     * NOTA:
                     * Questa eccezione non viene considerata perché significa che il codice chiamante
                     * ha tentato di iniziare una transaction senza considerare che occorre fare il binding della 
                     * sessione che si intende utilizzare, che serve per eseguire le operazioni verso il DataStore.
                     * */
                    #endregion
                }
                finally
                {
                    this.transactions.Clear();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Reset()
        {
            this.transactions.Clear();
        }

        
    }
}
