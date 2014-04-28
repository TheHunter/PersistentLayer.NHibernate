using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using PersistentLayer.NHibernate.Impl;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public static class NhDaoFactory
    {
        /// <summary>
        /// Builds a new INhPagedDAO using a contextual session provider.
        /// </summary>
        /// <param name="sessionFactory">the session factory used for building new sessions.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static INhPagedDAO MakePagedDAO(ISessionFactory sessionFactory)
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory", "The ISessionFacotry instance cannot be null.");

            return new EnterprisePagedDAO(new SessionContextProvider(sessionFactory.OpenSession));
        }

        /// <summary>
        /// Builds a new INhPagedDAO using a contextual session provider.
        /// </summary>
        /// <param name="sessionFactory">the session factory used for building new sessions.</param>
        /// <param name="context">a key used for comparing providers if application intends to used unique session providers by this information.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static INhPagedDAO MakePagedDAO(ISessionFactory sessionFactory, object context)
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory", "The ISessionFacotry instance cannot be null.");

            return new EnterprisePagedDAO(new SessionContextProvider(sessionFactory.OpenSession, context));
        }

        /// <summary>
        /// Builds a new INhPagedDAO using a contextual session provider.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="sessionFactory">the session factory used for building new sessions.</param>
        /// <returns></returns>
        public static INhPagedDAO<TEntity, TKey> MakePagedDAO<TEntity, TKey>(ISessionFactory sessionFactory)
            where TEntity : class
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory", "The ISessionFacotry instance cannot be null.");

            return new BusinessPagedDAO<TEntity, TKey>(new SessionContextProvider(sessionFactory.OpenSession));
        }

        /// <summary>
        /// Builds a new INhPagedDAO using a contextual session provider.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="sessionFactory">the session factory used for building new sessions.</param>
        /// <param name="context">a key used for comparing providers if application intends to used unique session providers by this information.</param>
        /// <returns></returns>
        public static INhPagedDAO<TEntity, TKey> MakePagedDAO<TEntity, TKey>(ISessionFactory sessionFactory, object context)
            where TEntity : class
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory", "The ISessionFacotry instance cannot be null.");

            return new BusinessPagedDAO<TEntity, TKey>(new SessionContextProvider(sessionFactory.OpenSession, context));
        }

        /// <summary>
        /// Builds a new INhPagedDAO using a contextual session provider.
        /// </summary>
        /// <typeparam name="TRootEntity"></typeparam>
        /// <param name="sessionFactory">the session factory used for building new sessions.</param>
        /// <returns></returns>
        public static INhRootPagedDAO<TRootEntity> MakeRootPagedDAO<TRootEntity>(ISessionFactory sessionFactory)
            where TRootEntity : class
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory", "The ISessionFacotry instance cannot be null.");

            return new EnterpriseRootDAO<TRootEntity>(new SessionContextProvider(sessionFactory.OpenSession));
        }

        /// <summary>
        /// Builds a new INhPagedDAO using a contextual session provider.
        /// </summary>
        /// <typeparam name="TRootEntity"></typeparam>
        /// <param name="sessionFactory">the session factory used for building new sessions.</param>
        /// <param name="context">a key used for comparing providers if application intends to used unique session providers by this information.</param>
        /// <returns></returns>
        public static INhRootPagedDAO<TRootEntity> MakeRootPagedDAO<TRootEntity>(ISessionFactory sessionFactory, object context)
            where TRootEntity : class
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory", "The ISessionFacotry instance cannot be null.");

            return new EnterpriseRootDAO<TRootEntity>(new SessionContextProvider(sessionFactory.OpenSession, context));
        }

        /// <summary>
        /// Builds a new INhPagedDAO using a contextual session provider.
        /// </summary>
        /// <typeparam name="TRootEntity"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sessionFactory">the session factory used for building new sessions.</param>
        /// <returns></returns>
        public static INhRootPagedDAO<TRootEntity, TEntity> MakeRootPagedDAO<TRootEntity, TEntity>(ISessionFactory sessionFactory)
            where TRootEntity : class
            where TEntity : class, TRootEntity
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory", "The ISessionFacotry instance cannot be null.");

            return new EnterpriseRootDAO<TRootEntity, TEntity>(new SessionContextProvider(sessionFactory.OpenSession));
        }

        /// <summary>
        /// Builds a new INhPagedDAO using a contextual session provider.
        /// </summary>
        /// <typeparam name="TRootEntity"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sessionFactory">the session factory used for building new sessions.</param>
        /// <param name="context">a key used for comparing providers if application intends to used unique session providers by this information.</param>
        /// <returns></returns>
        public static INhRootPagedDAO<TRootEntity, TEntity> MakeRootPagedDAO<TRootEntity, TEntity>(ISessionFactory sessionFactory, object context)
            where TRootEntity : class
            where TEntity : class, TRootEntity
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory", "The ISessionFacotry instance cannot be null.");

            return new EnterpriseRootDAO<TRootEntity, TEntity>(new SessionContextProvider(sessionFactory.OpenSession, context));
        }
    }
}
