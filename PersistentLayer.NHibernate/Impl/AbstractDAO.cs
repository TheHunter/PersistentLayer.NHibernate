using NHibernate;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractDAO
        : ISessionContext, ITransactionContext
    {
        private readonly SessionInfo sessionInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionProvider"></param>
        internal protected AbstractDAO(ISessionProvider sessionProvider)
        {
            this.sessionInfo = new SessionInfo(sessionProvider);
        }
        
        /// <summary>
        /// 
        /// </summary>
        SessionInfo ISessionContext.SessionInfo
        {
            get { return this.sessionInfo; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ISession CurrentSession
        {
            get { return this.sessionInfo.CurrentSession; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITransactionProvider GetTransactionProvider()
        {
            return this.sessionInfo.Provider;
        }

        /// <summary>
        /// 
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
