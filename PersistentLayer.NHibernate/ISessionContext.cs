using System;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Represents a contextual session accessible by this framework only.
    /// </summary>
    public interface ISessionContext
        : IDisposable
    {
        /// <summary>
        /// Session info used by this framework.
        /// </summary>
        SessionInfo SessionInfo { get; }

    }
}
