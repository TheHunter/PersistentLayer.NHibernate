using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Interface ISessionBinderProvider
    /// </summary>
    public interface ISessionBinderProvider
        : ISessionProvider
    {
        /// <summary>
        /// Binds the current session.
        /// </summary>
        void BindSession();

        /// <summary>
        /// Unbind the current session.
        /// </summary>
        void UnBindSession();

        /// <summary>
        /// Determines whether [has session binded].
        /// </summary>
        /// <returns><c>true</c> if [has session binded]; otherwise, <c>false</c>.</returns>
        bool HasSessionBinded();

    }
}
