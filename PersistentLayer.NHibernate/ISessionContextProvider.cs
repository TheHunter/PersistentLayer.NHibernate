using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISessionContextProvider
        : ISessionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        object KeyContext { get; }
    }
}
