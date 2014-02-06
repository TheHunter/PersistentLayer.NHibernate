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
    public interface ISessionBinderProvider
        : ISessionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ISession OpenSession();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="mode"></param>
        void BindSession(ISession session, FlushMode mode);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool HasSessionBinded();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ISession UnbindSession();

    }
}
