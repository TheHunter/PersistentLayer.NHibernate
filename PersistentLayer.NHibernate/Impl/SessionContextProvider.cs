using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Exceptions;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// Represents a session provider which is associated into particular context.
    /// </summary>
    public class SessionContextProvider
        : SessionProvider, ISessionContextProvider
    {
        private readonly object keyContext;
        private readonly Func<ISession> sessionSupplier;

        /// <summary>
        /// The default context
        /// </summary>
        public const string DefaultContext = "_defaultContext_";
        private ISession sessionCached;
        private bool wasDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionContextProvider"/> class.
        /// </summary>
        /// <param name="sessionSupplier">The session supplier.</param>
        public SessionContextProvider(Func<ISession> sessionSupplier)
            : this(sessionSupplier, DefaultContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionContextProvider"/> class.
        /// </summary>
        /// <param name="sessionSupplier">The session supplier.</param>
        /// <param name="keyContext">The key context.</param>
        /// <exception cref="PersistentLayer.Exceptions.BusinessLayerException">
        /// The delegate for retrieving / opening the session for the calling instance cannot be null.
        /// or
        /// The keyContext argument cannot be null;ctor SessionContextProvider;new ArgumentNullException(keyContext, The keyContext argument cannot be null)
        /// or
        /// The keyContext argument cannot be empty;ctor SessionContextProvider;new ArgumentNullException(keyContext, The keyContext argument cannot be empty)
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// keyContext;The keyContext argument cannot be null
        /// or
        /// keyContext;The keyContext argument cannot be empty
        /// </exception>
        /// <exception cref="PersistentLayer.Exceptions.SessionNotAvailableException">The session supplier returns a null reference.;ctor SessionDelegateProvider</exception>
        /// <exception cref="InvalidSessionException">The session supplier must return unique sessions, so no session could be recycled.;ctor SessionDelegateProvider</exception>
        public SessionContextProvider(Func<ISession> sessionSupplier, object keyContext)
        {
            if (sessionSupplier == null)
                throw new BusinessLayerException("The delegate for retreiving / opening the session for the calling instance cannot be null.");

            if (keyContext == null)
                throw new BusinessLayerException("The keyContext argument cannot be null", "ctor SessionContextProvider", new ArgumentNullException("keyContext", "The keyContext argument cannot be null"));

            this.sessionSupplier = sessionSupplier;

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

            this.wasDisposed = false;

            #region I verify the instance which is supplied by sessionSupplier reference.
            ISession s1 = this.sessionSupplier.Invoke();
            ISession s2 = this.sessionSupplier.Invoke();

            if (s1 == null)
                throw new SessionNotAvailableException("The session supplier returns a null reference.", "ctor SessionDelegateProvider");

            if (s1 == s2 || s1.Equals(s2))
                throw new InvalidSessionException("The session supplier must return unique sessions, so no session could be recycled.", "ctor SessionDelegateProvider");

            s1.Dispose();
            if (s2 != null)
                s2.Dispose();
            #endregion
        }

        /// <summary>
        /// Gets the key context.
        /// </summary>
        /// <value>The key context.</value>
        public object KeyContext
        {
            get { return keyContext; }
        }

        /// <summary>
        /// Gets the current bounded session by a higher implementation level.
        /// </summary>
        /// <returns>Returns the current binded session by a higher implementation level.</returns>
        /// <exception cref="PersistentLayer.Exceptions.SessionNotAvailableException">
        /// There's no suitable session for making CRUD operations because the calling instance was disposed.;GetCurrentSession
        /// or
        /// Error on opening a new session for the calling session provider instance.;GetCurrentSession
        /// or
        /// Error on retrieving the session, see inner exception for details.;GetCurrentSession
        /// </exception>
        /// <exception cref="PersistentLayer.Exceptions.SessionNotOpenedException">The session associated into the calling instance must be opened.</exception>
        public override ISession GetCurrentSession()
        {
            if (wasDisposed)
                throw new SessionNotAvailableException("There's no suitable session for making CRUD operations because the calling instance was disposed.", "GetCurrentSession");

            try
            {
                if (this.sessionCached == null)
                {
                    this.sessionCached = this.sessionSupplier.Invoke();
                    if (this.sessionCached == null)
                        throw new SessionNotAvailableException("Error on opening a new session for the calling session provider instance.", "GetCurrentSession");

                }
                
                if (!this.sessionCached.IsOpen)
                    throw new SessionNotOpenedException("The session associated into the calling instance must be opened.");

                return this.sessionCached;
            }
            catch (Exception ex)
            {
                throw new SessionNotAvailableException("Error on retrieving the session, see inner exception for details.", "GetCurrentSession", ex);
            }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            this.wasDisposed = true;

            base.Dispose();
            this.Reset();
        }

        /// <summary>
        /// Clear all internal transactions.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();
            if (this.sessionCached != null)
            {
                if (this.sessionCached.IsOpen)
                    this.sessionCached.Close();

                this.sessionCached = null;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is SessionContextProvider)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.keyContext.GetHashCode();
        }
    }
}
