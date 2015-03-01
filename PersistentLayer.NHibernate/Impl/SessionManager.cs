using System;
using System.Collections.Generic;
using NHibernate;
using System.Data;
using System.Linq;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// Manages the session factory in order to open/manage Sessions.
    /// </summary>
    [Serializable]
    public class SessionManager
        : SessionProvider, ISessionManager
    {
        private readonly ISessionFactory sessionFactory;

        #region Session factory section

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionManager"/> class.
        /// </summary>
        /// <param name="sessionFactory">The session factory.</param>
        /// <exception cref="PersistentLayer.Exceptions.BusinessLayerException">The SessionFactory for SessionManager cannot be null.;ctor SessionManager</exception>
        public SessionManager(ISessionFactory sessionFactory)
        {
            if (sessionFactory == null)
                throw new BusinessLayerException("The SessionFactory for SessionManager cannot be null.", "ctor SessionManager");

            this.sessionFactory = sessionFactory;
        }

        #endregion

        /// <summary>
        /// Gets the session factory.
        /// </summary>
        /// <value>The session factory.</value>
        public ISessionFactory SessionFactory
        {
            get { return this.sessionFactory; }
        }

        /// <summary>
        /// Gets the current bounded session by a higher implementation level.
        /// </summary>
        /// <returns>Returns the current binded session by a higher implementation level.</returns>
        /// <exception cref="PersistentLayer.Exceptions.SessionNotBindedException">There's no binded session, so first It would require to open a new session.;GetCurrentSession</exception>
        public override ISession GetCurrentSession()
        {
            try
            {
                return this.sessionFactory.GetCurrentSession();
            }
            catch (Exception ex)
            {
                this.Reset();
                throw new SessionNotBindedException("There's no binded session, so first It would require to open a new session.", "GetCurrentSession", ex);
            }
        }

    }
}
