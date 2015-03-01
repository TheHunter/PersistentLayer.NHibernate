using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Interface ISessionContextProvider
    /// </summary>
    public interface ISessionContextProvider
        : ISessionProvider
    {
        /// <summary>
        /// Gets the key context.
        /// </summary>
        /// <value>The key context.</value>
        object KeyContext { get; }
    }
}
