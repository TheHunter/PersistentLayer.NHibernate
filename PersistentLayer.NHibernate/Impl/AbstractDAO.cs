using NHibernate;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractDAO
        : ISessionContext, ITransactionContext
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract SessionInfo SessionInfo { get; }

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
