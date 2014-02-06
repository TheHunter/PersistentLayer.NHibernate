using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISessionManager
        : ISessionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        ISessionFactory SessionFactory { get; }
    }
}
