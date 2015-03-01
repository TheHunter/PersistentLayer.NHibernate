using System;
using NHibernate;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Class SessionInfo.
    /// </summary>
    public class SessionInfo
    {
        private readonly ISessionProvider sessionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionInfo"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException">provider;Error on assigning the session provider into BussinesDAO 'cause It's null.</exception>
        public SessionInfo(ISessionProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider", "Error on assigning the session provider into BussinesDAO 'cause It's null.");
            
            this.sessionProvider = provider;
        }

        /// <summary>
        /// Gets the current session.
        /// </summary>
        /// <value>The current session.</value>
        internal protected ISession CurrentSession
        {
            get { return this.sessionProvider.GetCurrentSession(); }
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>The provider.</value>
        internal protected ISessionProvider Provider
        {
            get { return this.sessionProvider; }
        }
    }
}
