using System;
using System.Collections.Generic;
using NHibernate;
using System.Data;
using System.Linq;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// Manages the session factory in order to open/manage Sessions.
    /// </summary>
    [Serializable]
    public class SessionManager
        : SessionProvider, ISessionManager
    {
        private readonly Stack<ITransactionInfo> transactions;
        /// <summary>
        /// This is the factory which creates sessions, and It's able to reference the current binded session
        /// made by CurrentSessionContext
        /// </summary>
        private readonly ISessionFactory sessionFactory;
        private const string DefaultNaming = "anonymous";

        #region Session factory section

        /// <summary>
        /// 
        /// </summary>
        protected SessionManager()
        {
            transactions = new Stack<ITransactionInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <exception cref="BusinessLayerException"></exception>
        public SessionManager(ISessionFactory sessionFactory)
        {
            transactions = new Stack<ITransactionInfo>();

            if (sessionFactory == null)
                throw new BusinessLayerException("The SessionFactory for SessionManager cannot be null.", "ctor SessionManager");

            this.sessionFactory = sessionFactory;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ISessionFactory SessionFactory
        {
            get { return this.sessionFactory; }
        }

        /// <summary>
        /// Gets the current binded session from the calling session manager.
        /// </summary>
        /// <returns>returns the current binded session</returns>
        /// <exception cref="SessionNotBindedException">
        /// Throws an exception when there's no session binded into any CurrentSessionContext.
        /// </exception>
        public override ISession GetCurrentSession()
        {
            try
            {
                return this.sessionFactory.GetCurrentSession();
            }
            catch (Exception ex)
            {
                this.transactions.Clear();
                throw new SessionNotBindedException("There's no binded session, so first It would require to open a new session.", "GetCurrentSession", ex);
            }
        }

        
    }
}
