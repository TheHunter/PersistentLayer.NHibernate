using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionalDAO
        : ITransactionContext
    {
        private readonly SessionInfo sessionInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionProvider"></param>
        internal protected TransactionalDAO(ISessionProvider sessionProvider)
        {
            this.sessionInfo = new SessionInfo(sessionProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        protected SessionInfo SessionInfo
        {
            get { return this.sessionInfo; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ISession CurrentSession
        {
            get { return this.SessionInfo.CurrentSession; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITransactionProvider GetTransactionProvider()
        {
            return this.SessionInfo.Provider;
        }
    }
}
