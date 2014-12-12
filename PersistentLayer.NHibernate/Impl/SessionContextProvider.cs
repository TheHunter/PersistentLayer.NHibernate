﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Exceptions;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionContextProvider
        : SessionProvider, ISessionContextProvider
    {
        private readonly object keyContext;
        private readonly Func<ISession> sessionSupplier;
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultContext = "_defaultContext_";
        private ISession sessionCached;
        private bool wasDisposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionSupplier"></param>
        public SessionContextProvider(Func<ISession> sessionSupplier)
            : this(sessionSupplier, DefaultContext)
        {
        }

        /// <summary>
        /// 
        /// </summary>
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
        /// 
        /// </summary>
        public override void Dispose()
        {
            this.wasDisposed = true;

            base.Dispose();
            this.Reset();
        }

        /// <summary>
        /// Clear all internal transactions and close current session.
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
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is SessionContextProvider)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.keyContext.GetHashCode();
        }
    }
}