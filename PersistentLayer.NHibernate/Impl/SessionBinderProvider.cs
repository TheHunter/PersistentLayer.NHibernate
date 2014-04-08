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
        : SessionManager, ISessionBinderProvider
    {

        //private Action<ISession> bindAction;
        //private Func<ISessionFactory, ISession> unBindAction;
        //private Func<ISessionFactory, bool> hasBind;

        /// <summary>
        /// A delegate which retrieves the session to use for binding into current context.
        /// </summary>
        private readonly Func<ISession> onRetrievingSession;
        /// <summary>
        /// A delegate which can be executed after retrieving the session to bind. 
        /// </summary>
        private readonly Action<ISession> beforeBinding;
        /// <summary>
        /// A delegate which will be executed after UnBinding the session binded from CurrentSessionContext.
        /// </summary>
        private readonly Action<ISession> afterUnBinding;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <param name="onRetrievingSession"></param>
        /// <param name="beforeBinding"></param>
        /// <param name="afterUnBinding"></param>
        public SessionBinderProvider(ISessionFactory sessionFactory, Func<ISession> onRetrievingSession, Action<ISession> beforeBinding, Action<ISession> afterUnBinding)
            : base(sessionFactory)
        {
            if (onRetrievingSession == null)
                throw new BusinessLayerException("The delegate to retrieve the session to bind cannot be null.", "ctor SessionBinderProvider");

            if (afterUnBinding == null)
                throw new BusinessLayerException("The delegate to unbind the session cannot be null.", "ctor SessionBinderProvider");

            this.onRetrievingSession = onRetrievingSession;
            this.beforeBinding = beforeBinding;
            this.afterUnBinding = afterUnBinding;
        }
        

        //private void OnInit()
        //{
        //    var flags = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;
        //    var type = typeof(TBinder);
        //    bindAction = (Action<ISession>)Delegate.CreateDelegate(typeof(Action<ISession>), null, type.GetMethod("Bind", flags));
        //    unBindAction = (Func<ISessionFactory, ISession>)Delegate.CreateDelegate(typeof(Func<ISessionFactory, ISession>), null, type.GetMethod("Unbind", flags));
        //    hasBind = (Func<ISessionFactory, bool>)Delegate.CreateDelegate(typeof(Func<ISessionFactory, bool>), null, type.GetMethod("HasBind", flags));
        //}

        
        //public ISession OpenSession()
        //{
        //    try
        //    {
        //        return this.SessionFactory.OpenSession();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new SessionNotOpenedException("Error on trying to open a new session, verify if the current database connection is available.", "OpenSession", ex);
        //    }
        //}

        
        //public void BindSession(ISession session, FlushMode mode)
        //{
        //    if (session != null)
        //    {
        //        if (!session.IsOpen)
        //        {
        //            throw new InvalidSessionException("The session to bind is closed, so It's required to open a new session.", "BindSession");
        //        }
        //    }
        //    else
        //    {
        //        throw new SessionNotAvailableException("There's no available session to bind into current context, It's require to open a new session.", "BindSession");
        //    }
        //    session.FlushMode = mode;
        //    CurrentSessionContext.Bind(session);
        //}

        /// <summary>
        /// 
        /// </summary>
        public void BindSession()
        {
            ISession session;
            try
            {
                session = this.onRetrievingSession.Invoke();
            }
            catch (Exception ex)
            {
                throw new BusinessLayerException("Error before invoking the delegate which retrives the session to bind, see inner exception for details.", "BindSession", ex);
            }

            if (session == null)
                throw new SessionNotAvailableException(
                    "There's no available session to bind into current context, It's require to open a new session.", "BindSession");

            if (!session.IsOpen)
                throw new InvalidSessionException(
                    "The session to bind is closed, so It's required to open a new session.", "BindSession");

            if (this.beforeBinding != null)
                this.beforeBinding.Invoke(session);

            CurrentSessionContext.Bind(session);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasSessionBinded()
        {
            return CurrentSessionContext.HasBind(this.SessionFactory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void UnBindSession()
        {
            ISession session = CurrentSessionContext.Unbind(this.SessionFactory);
            this.afterUnBinding(session);
        }

    }
}
