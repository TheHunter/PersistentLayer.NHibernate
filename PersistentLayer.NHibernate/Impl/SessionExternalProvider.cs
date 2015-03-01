using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistentLayer.NHibernate.Exceptions;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// Class SessionExternalProvider.
    /// </summary>
    public class SessionExternalProvider
        : SessionProvider
    {
        private readonly ISession externalSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionExternalProvider"/> class.
        /// </summary>
        /// <param name="externalSession">The external session.</param>
        public SessionExternalProvider(ISession externalSession)
        {
            if (externalSession == null)
                throw new InvalidSessionException("The given session cannot be null.");

            this.externalSession = externalSession;
        }

        /// <summary>
        /// Gets the current bounded session by a higher implementation level.
        /// </summary>
        /// <returns>Returns the current binded session by a higher implementation level.</returns>
        public override ISession GetCurrentSession()
        {
            return externalSession;
        }
    }
}
