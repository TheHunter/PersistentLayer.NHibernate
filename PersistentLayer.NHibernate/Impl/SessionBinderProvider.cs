using System;
using NHibernate;
using NHibernate.Context;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// A session manager which gets binded and unbinded sessions.
    /// </summary>
    public class SessionBinderProvider
        : SessionProvider, ISessionBinderProvider
    {
        /// <summary>
        /// A delegate which retrieves the session to use for binding into current context.
        /// </summary>
        private readonly Func<ISession> onRetrievingSession;
        /// <summary>
        /// A delegate which can be executed whenever a session must be binded into user custom context. 
        /// </summary>
        private readonly Action onBinding;
        /// <summary>
        /// A delegate which will be executed whenever a session must be unbinded from user custom context.
        /// </summary>
        private readonly Action onUnBinding;
        /// <summary>
        /// A delegate which verifies if exists any session binded into user custom context.
        /// </summary>
        private readonly Func<bool> hasSessionBinded;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionBinderProvider"/> class.
        /// </summary>
        /// <param name="onRetrievingSession">The on retrieving session.</param>
        /// <param name="onBinding">The on binding.</param>
        /// <param name="onUnBinding">The on un binding.</param>
        /// <param name="hasSessionBinded">The has session binded.</param>
        /// <exception cref="PersistentLayer.Exceptions.BusinessLayerException">
        /// The delegate to retrieve the session to bind cannot be null.;ctor SessionBinderProvider
        /// or
        /// The delegate to unbind the session cannot be null.;ctor SessionBinderProvider
        /// </exception>
        public SessionBinderProvider(Func<ISession> onRetrievingSession, Action onBinding, Action onUnBinding, Func<bool> hasSessionBinded)
        {
            if (onRetrievingSession == null)
                throw new BusinessLayerException("The delegate to retrieve the session to bind cannot be null.", "ctor SessionBinderProvider");

            if (onUnBinding == null)
                throw new BusinessLayerException("The delegate to unbind the session cannot be null.", "ctor SessionBinderProvider");

            this.onRetrievingSession = onRetrievingSession;
            this.onBinding = onBinding;
            this.onUnBinding = onUnBinding;
            this.hasSessionBinded = hasSessionBinded;
        }

        /// <summary>
        /// Gets the current bounded session by a higher implementation level.
        /// </summary>
        /// <returns>Returns the current binded session by a higher implementation level.</returns>
        /// <exception cref="System.NullReferenceException">The session retrieved is null.</exception>
        /// <exception cref="PersistentLayer.Exceptions.SessionNotOpenedException">Session retrieved is closed.</exception>
        /// <exception cref="PersistentLayer.Exceptions.SessionNotBindedException">There's no binded session, so first It would require to open a new session.;GetCurrentSession</exception>
        public override ISession GetCurrentSession()
        {
            try
            {
                ISession session =  this.onRetrievingSession.Invoke();
                if (session == null)
                    throw new NullReferenceException("The session retrieved is null.");

                if (!session.IsOpen)
                    throw new SessionNotOpenedException("Session retrieved is closed.");

                return session;
            }
            catch (Exception ex)
            {
                throw new SessionNotBindedException("There's no binded session, so first It would require to open a new session.", "GetCurrentSession", ex);
            }
            
        }

        /// <summary>
        /// Binds the current session.
        /// </summary>
        /// <exception cref="PersistentLayer.Exceptions.BusinessLayerException">Error on invoking the delegate which binds the session, see inner exception for details.;BindSession</exception>
        public void BindSession()
        {
            try
            {
                this.onBinding.Invoke();
            }
            catch (Exception ex)
            {
                throw new BusinessLayerException("Error on invoking the delegate which binds the session, see inner exception for details.", "BindSession", ex);
            }

        }

        /// <summary>
        /// Determines whether [has session binded].
        /// </summary>
        /// <returns><c>true</c> if [has session binded]; otherwise, <c>false</c>.</returns>
        /// <exception cref="PersistentLayer.Exceptions.BusinessLayerException">Error on invoking the delegate which verifies if exists any session binded, see inner exception for details.;HasSessionBinded</exception>
        public bool HasSessionBinded()
        {
            try
            {
                return this.hasSessionBinded();
            }
            catch (Exception ex)
            {

                throw new BusinessLayerException("Error on invoking the delegate which verifies if exists any session binded, see inner exception for details.", "HasSessionBinded", ex);
            }
        }

        /// <summary>
        /// Unbind the current session.
        /// </summary>
        /// <exception cref="PersistentLayer.Exceptions.BusinessLayerException">Error on invoking the delegate which unbinds the session, see inner exception for details.;UnBindSession</exception>
        public void UnBindSession()
        {
            try
            {
                this.onUnBinding.Invoke();
            }
            catch (Exception ex)
            {
                throw new BusinessLayerException("Error on invoking the delegate which unbinds the session, see inner exception for details.", "UnBindSession", ex);
            }
        }

    }
}
