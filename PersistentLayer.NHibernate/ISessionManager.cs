using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Interface ISessionManager
    /// </summary>
    public interface ISessionManager
        : ISessionProvider
    {
        /// <summary>
        /// Gets the session factory.
        /// </summary>
        /// <value>The session factory.</value>
        ISessionFactory SessionFactory { get; }
    }
}
