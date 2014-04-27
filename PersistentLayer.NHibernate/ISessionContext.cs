using System;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISessionContext
        : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        SessionInfo SessionInfo { get; }

    }
}
