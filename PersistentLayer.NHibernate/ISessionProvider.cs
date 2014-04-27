using System;
using NHibernate;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Provides sessions by a higher implementation.
    /// </summary>
    public interface ISessionProvider
        : ITransactionProvider, IDisposable
    {
        /// <summary>
        /// Gets the current bounded session by a higher implementation level.
        /// </summary>
        /// <returns>Returns the current binded session by a higher implementation level.</returns>
        /// <exception cref="SessionNotBindedException">
        /// Throws an exception when there's no session binded into any CurrentSessionContext.
        /// </exception>
        ISession GetCurrentSession();
    }
}
