using NHibernate;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// Class AbstractDAO.
    /// </summary>
    public abstract class AbstractDAO
        : ISessionContext, ITransactionContext
    {
        private readonly SessionInfo sessionInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDAO"/> class.
        /// </summary>
        /// <param name="sessionProvider">The session provider.</param>
        internal protected AbstractDAO(ISessionProvider sessionProvider)
        {
            this.sessionInfo = new SessionInfo(sessionProvider);
        }

        /// <summary>
        /// Session info used by this framework.
        /// </summary>
        /// <value>The session information.</value>
        SessionInfo ISessionContext.SessionInfo
        {
            get { return this.sessionInfo; }
        }

        /// <summary>
        /// Gets the current session.
        /// </summary>
        /// <value>The current session.</value>
        protected ISession CurrentSession
        {
            get { return this.sessionInfo.CurrentSession; }
        }

        /// <summary>
        /// Gets the transaction provider.
        /// </summary>
        /// <returns>ITransactionProvider.</returns>
        public ITransactionProvider GetTransactionProvider()
        {
            return this.sessionInfo.Provider;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (this.sessionInfo != null && this.sessionInfo.Provider != null)
            {
                this.sessionInfo
                    .Provider
                    .Dispose();
            }
        }
    }
}
