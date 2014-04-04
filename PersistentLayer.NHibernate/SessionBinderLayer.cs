using System;
using NHibernate;
using System.Reflection;
using NHibernate.Context;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// A session manager which gets binded and unbinded sessions.
    /// </summary>
    public class SessionBinderLayer
        : SessionManager, ISessionBinderProvider
        //where TBinder : CurrentSessionContext
    {

        //private Action<ISession> bindAction;
        //private Func<ISessionFactory, ISession> unBindAction;
        //private Func<ISessionFactory, bool> hasBind;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactory"></param>
        public SessionBinderLayer(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
            //this.OnInit();
        }

        
        //private void OnInit()
        //{
        //    var flags = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;
        //    var type = typeof(TBinder);
        //    bindAction = (Action<ISession>)Delegate.CreateDelegate(typeof(Action<ISession>), null, type.GetMethod("Bind", flags));
        //    unBindAction = (Func<ISessionFactory, ISession>)Delegate.CreateDelegate(typeof(Func<ISessionFactory, ISession>), null, type.GetMethod("Unbind", flags));
        //    hasBind = (Func<ISessionFactory, bool>)Delegate.CreateDelegate(typeof(Func<ISessionFactory, bool>), null, type.GetMethod("HasBind", flags));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="SessionNotOpenedException"></exception>
        /// <returns></returns>
        public ISession OpenSession()
        {
            try
            {
                return this.SessionFactory.OpenSession();
            }
            catch (Exception ex)
            {
                throw new SessionNotOpenedException("Error on trying to open a new session, verify if the current database connection is available.", "OpenSession", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="mode"></param>
        /// <exception cref="SessionNotAvailableException"></exception>
        /// <exception cref="InvalidSessionException"></exception>
        public void BindSession(ISession session, FlushMode mode)
        {
            if (session != null)
            {
                if (!session.IsOpen)
                {
                    throw new InvalidSessionException("The session to bind is closed, so It's required to open a new session.", "BindSession");
                }
            }
            else
            {
                throw new SessionNotAvailableException("There's no available session to bind into current context, It's require to open a new session.", "BindSession");
            }
            session.FlushMode = mode;
            //this.bindAction(session);
            CurrentSessionContext.Bind(session);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasSessionBinded()
        {
            //return this.hasBind(this.SessionFactory);
            return CurrentSessionContext.HasBind(this.SessionFactory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISession UnbindSession()
        {
            //return this.unBindAction(this.SessionFactory);
            return CurrentSessionContext.Unbind(this.SessionFactory);
        }

    }
}
