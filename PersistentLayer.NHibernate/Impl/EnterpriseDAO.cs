namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class EnterpriseDAO
        : DomainDAO, ISessionDAO
    {
        private readonly SessionInfo sessionInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionProvider"></param>
        public EnterpriseDAO(ISessionProvider sessionProvider)
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
