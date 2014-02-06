namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="VKey"></typeparam>
    public class BusinessDAO<TEntity, VKey>
        : EntityDAO<TEntity, VKey>, ISessionDAO<TEntity, VKey>
        where TEntity : class
    {
        private readonly SessionInfo sessionInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionProvider"></param>
        public BusinessDAO(ISessionProvider sessionProvider)
        {
            sessionInfo = new SessionInfo(sessionProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        public override SessionInfo SessionInfo
        {
            get { return this.sessionInfo; }
        }

    }
}
