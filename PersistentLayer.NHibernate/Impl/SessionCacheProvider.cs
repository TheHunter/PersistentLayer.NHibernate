using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// Manages a contextual session during the its life cycle, and opening new sessions after making rollback / commit transactions.
    /// </summary>
    public class SessionCacheProvider
        : SessionContextProvider
    {
        private readonly bool newSessionAfterCommit;
        private readonly bool newSessionAfterRollback;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionCacheProvider"/> class.
        /// </summary>
        /// <param name="sessionSupplier">The session supplier.</param>
        /// <param name="newSessionAfterCommit">if set to <c>true</c> [new session after commit].</param>
        /// <param name="newSessionAfterRollback">if set to <c>true</c> [new session after rollback].</param>
        public SessionCacheProvider(Func<ISession> sessionSupplier, bool newSessionAfterCommit, bool newSessionAfterRollback)
            : this(sessionSupplier, DefaultContext, newSessionAfterCommit, newSessionAfterRollback)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionCacheProvider"/> class.
        /// </summary>
        /// <param name="sessionSupplier">The session supplier.</param>
        /// <param name="keyContext">The key context.</param>
        /// <param name="newSessionAfterCommit">if set to <c>true</c> [new session after commit].</param>
        /// <param name="newSessionAfterRollback">if set to <c>true</c> [new session after rollback].</param>
        public SessionCacheProvider(Func<ISession> sessionSupplier, object keyContext, bool newSessionAfterCommit, bool newSessionAfterRollback)
            : base(sessionSupplier, keyContext)
        {
            this.newSessionAfterCommit = newSessionAfterCommit;
            this.newSessionAfterRollback = newSessionAfterRollback;
        }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        public override void CommitTransaction()
        {
            base.CommitTransaction();
            if (this.newSessionAfterCommit && !this.InProgress)
                this.Reset();
        }

        /// <summary>
        /// Makes a rollback the transaction.
        /// </summary>
        public override void RollbackTransaction()
        {
            this.RollbackTransaction(null);
        }

        /// <summary>
        /// Makes a rollback, indicating the exception associated to the last transaction.
        /// </summary>
        /// <param name="cause">The cause.</param>
        public override void RollbackTransaction(Exception cause)
        {
            base.RollbackTransaction(cause);
            if (this.newSessionAfterRollback)
                this.Reset();
        }

       
    }
}
