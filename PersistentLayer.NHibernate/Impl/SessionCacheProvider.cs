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
    public class SessionCacheProvider
        : SessionDelegateProvider
    {
        private readonly bool newSessionAfterCommit;
        private readonly bool newSessionAfterRollback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionSupplier"></param>
        /// <param name="newSessionAfterCommit"></param>
        /// <param name="newSessionAfterRollback"></param>
        public SessionCacheProvider(Func<ISession> sessionSupplier, bool newSessionAfterCommit, bool newSessionAfterRollback)
            : this(sessionSupplier, DefaultContext, newSessionAfterCommit, newSessionAfterRollback)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionSupplier"></param>
        /// <param name="keyContext"></param>
        /// <param name="newSessionAfterCommit"></param>
        /// <param name="newSessionAfterRollback"></param>
        public SessionCacheProvider(Func<ISession> sessionSupplier, object keyContext, bool newSessionAfterCommit, bool newSessionAfterRollback)
            : base(sessionSupplier, keyContext)
        {
            this.newSessionAfterCommit = newSessionAfterCommit;
            this.newSessionAfterRollback = newSessionAfterRollback;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CommitTransaction()
        {
            base.CommitTransaction();
            if (this.newSessionAfterCommit)
                this.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void RollbackTransaction()
        {
            this.RollbackTransaction(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cause"></param>
        public override void RollbackTransaction(Exception cause)
        {
            base.RollbackTransaction(cause);
            if (this.newSessionAfterRollback)
                this.Reset();
        }
       
    }
}
