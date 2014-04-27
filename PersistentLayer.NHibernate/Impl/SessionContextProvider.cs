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
        : SessionProvider, ISessionContextProvider
    {
        private readonly ISession session;
        private readonly object keyContext;
        protected const string DefaultContext = "_defaultContext_";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public SessionContextProvider(ISession session)
            : this(session, DefaultContext)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="keyContext"></param>
        public SessionContextProvider(ISession session, object keyContext)
        {
            if (session == null)
                throw new SessionNotAvailableException("The session to associate into SessionContextProvider instance cannot be null.", "ctor SessionContextProvider");

            if (!session.IsOpen)
                throw new InvalidSessionException("There's no suitable session for making CRUD operations because the session parameter is closed.", "ctor SessionContextProvider");

            this.session = session;

            if (keyContext == null)
                throw new BusinessLayerException("The keyContext argument cannot be null", "ctor SessionContextProvider", new ArgumentNullException("keyContext", "The keyContext argument cannot be null"));

            string str = keyContext as string;
            if (str != null)
            {
                if (str.Trim().Equals(string.Empty))
                    throw new BusinessLayerException("The keyContext argument cannot be empty", "ctor SessionContextProvider", new ArgumentNullException("keyContext", "The keyContext argument cannot be empty"));

                this.keyContext = str.Trim();
            }
            else
            {
                this.keyContext = keyContext;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object KeyContext
        {
            get { return keyContext; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ISession GetCurrentSession()
        {
            if (!this.session.IsOpen )
                throw new InvalidSessionException("There's no suitable session for making CRUD operations because the calling instance was disposed.", "GetCurrentSession");
            
            return this.session;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            if (this.session.IsOpen)
                this.session.Close();
        }
    }
}
