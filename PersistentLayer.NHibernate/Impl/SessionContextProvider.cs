using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionContextProvider
        : SessionProvider, IDisposable
    {
        private readonly ISession session;

        #region Session factory section

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public SessionContextProvider(ISession session)
        {
            if (session == null)
                throw new SessionNotAvailableException("The session to associate into SessionContextProvider instance cannot be null.");

            this.session = session;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ISession GetCurrentSession()
        {
            if (this.session == null || !this.session.IsOpen )
            {
                this.Reset();
                throw new InvalidSessionException("There's no suitable session for making CRUD operations because the calling instance was disposed.", "GetCurrentSession");
            }
            return this.session;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Reset();
            if (this.session != null)
            {
                if (this.session.IsOpen)
                {
                    this.session.Close();
                }
            }
        }
    }
}
