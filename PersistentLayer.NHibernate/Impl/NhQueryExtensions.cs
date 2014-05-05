using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using NHibernate.Metadata;
using NHibernate.Transform;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// Extensions methods for DAO's.
    /// </summary>
    public static class NhQueryExtensions
    {
        /// <summary>
        /// Converts lambda tree expression into IFutureValue object.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        internal static IFutureValue<TResult> ToFutureValue<TSource, TResult>
            (this IQueryable<TSource> source, Expression<Func<IQueryable<TSource>, TResult>> selector)
            where TResult : struct
        {
            var provider = (INhQueryProvider)source.Provider;
            var method = ((MethodCallExpression)selector.Body).Method;
            var expression = System.Linq.Expressions.Expression.Call(null, method, source.Expression);
            return (IFutureValue<TResult>)provider.ExecuteFuture(expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="instance"></param>
        /// <exception cref="BusinessPersistentException"></exception>
        /// <returns></returns>
        public static TEntity Merge<TEntity>
            (this ISessionContext sourceDAO, TEntity instance)
            where TEntity : class
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;
            try
            {
                return session.Merge(instance);
            }
            catch (Exception ex)
            {
                throw new BusinessPersistentException("Error on merging the instance with the current session.", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="instance"></param>
        /// <exception cref="BusinessPersistentException"></exception>
        /// <returns></returns>
        public static object Merge
            (this ISessionContext sourceDAO, object instance)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;
            try
            {
                return session.Merge(instance);
            }
            catch (Exception ex)
            {
                throw new BusinessPersistentException("Error on merging the instance with the current session.", ex);
            }
        }

        /// <summary>
        /// Indicates if the argument is present into current session cache.
        /// </summary>
        /// <typeparam name="TEntity">Persistence instance</typeparam>
        /// <param name="sourceDAO">DAO which is associated into the current session.</param>
        /// <param name="instance">persistence instance to check.</param>
        /// <returns>returns a boolean value indicating if persistent entity is present into current session cache.</returns>
        public static bool IsCached<TEntity>
            (this ISessionContext sourceDAO, TEntity instance)
            where TEntity : class
        {
            return sourceDAO.IsCached(instance as object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsCached
            (this ISessionContext sourceDAO, object instance)
        {
            return sourceDAO.SessionInfo.CurrentSession.Contains(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="instances"></param>
        /// <returns></returns>
        public static bool IsCached<TEntity>
            (this ISessionContext sourceDAO, IEnumerable<TEntity> instances)
            where TEntity : class
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (instances != null)
                return instances.All(session.Contains);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="instances"></param>
        /// <returns></returns>
        public static bool IsCached
            (this ISessionContext sourceDAO, IEnumerable instances)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (instances != null)
                return instances.Cast<object>().All(session.Contains);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="instance"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static TKey GetIdentifier<TEntity, TKey>
            (this ISessionContext sourceDAO, TEntity instance)
            where TEntity : class
        {
            return (TKey)sourceDAO.GetIdentifier(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="instance"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static object GetIdentifier
            (this ISessionContext sourceDAO, object instance)
        {
            if (instance == null)
                throw new QueryArgumentException("Instance to analize for getting its identifier cannot be null.", "GetIdentifier", "instance");
            try
            {
                return sourceDAO.SessionInfo.CurrentSession.GetIdentifier(instance);
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on getting the identifier of the given instance.", "GetIdentifier", ex);
            }
        }

        /// <summary>
        /// Indicates if the current session contains any changes which must be synchronized with the database.
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <returns>returns a boolean value indicating if the current session associated with the calling DAO contains any changes to persist.</returns>
        public static bool SessionWithChanges
            (this ISessionContext sourceDAO)
        {
            return sourceDAO.SessionInfo.CurrentSession.IsDirty();
        }

        /// <summary>
        /// Evicts the persistent instance from session cache, if instance is cached into current session.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="instance"></param>
        public static void Evict<TEntity>
            (this ISessionContext sourceDAO, TEntity instance)
            where TEntity : class
        {
            sourceDAO.Evict(instance as object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="instance"></param>
        public static void Evict
            (this ISessionContext sourceDAO, object instance)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (instance != null && sourceDAO.IsCached(instance))
            {
                try
                {
                    session.Evict(instance);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Evicts all instances present into collection argument, if instances are cached into current session.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="instances"></param>
        public static void Evict<TEntity>
            (this ISessionContext sourceDAO, IEnumerable<TEntity> instances)
            where TEntity : class
        {
            if (instances != null && instances.Any())
            {
                instances.All
                    (
                        instance =>
                            {
                                sourceDAO.Evict(instance);
                                return true;
                            }
                    );
            }
        }

        /// <summary>
        /// Evict all persistent instances from the current session cache.
        /// </summary>
        /// <param name="sourceDAO">DAO associated with session wich be clear.</param>
        public static void Evict
            (this ISessionContext sourceDAO)
        {
            sourceDAO.SessionInfo.CurrentSession.Clear();
        }

        /// <summary>
        /// Force the current ISession to flush.
        /// </summary>
        /// <param name="sourceDAO"></param>
        public static void Flush
            (this ISessionContext sourceDAO)
        {
            sourceDAO.SessionInfo.CurrentSession.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <exception cref="QueryArgumentException"></exception>
        /// <returns></returns>
        public static object FindBy
            (this ISessionContext sourceDAO, Type type, object id)
        {
            return sourceDAO.FindBy(type, id, LockMode.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static object FindBy
            (this ISessionContext sourceDAO, Type type, object id, LockMode mode)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (id == null)
                throw new QueryArgumentException("The given identifier cannot be null", "FindBy", "id");

            if (type == null)
                throw new QueryArgumentException("The object type cannot be null.", "FindBy", "type");
            
            try
            {
                if (mode == null) mode = LockMode.None;
                
                return session.Get(type, id, mode);
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on getting the persistent instance with the given indetifier.", "FindBy", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static object Load
            (this ISessionContext sourceDAO, Type type, object id, LockMode mode)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (id == null)
                throw new QueryArgumentException("The given identifier cannot be null", "Load", "id");

            if (type == null)
                throw new QueryArgumentException("The object type cannot be null.", "Load", "type");

            try
            {
                if (mode == null) mode = LockMode.None;

                return session.Load(type, id, mode);
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on loading the persistent instance with the given indetifier.", "Load", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TEntity Load<TEntity>
            (this ISessionContext sourceDAO, object id) where TEntity : class
        {
            return sourceDAO.Load<TEntity>(id, LockMode.Read);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static TEntity Load<TEntity>
            (this ISessionContext sourceDAO, object id, LockMode mode) where TEntity : class
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (id == null)
                throw new QueryArgumentException("The given identifier cannot be null", "Load", "id");

            try
            {
                if (mode == null) mode = LockMode.None;

                return session.Load<TEntity>(id, mode);
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on loading the persistent instance with the given indetifier.", "Load", ex);
            }
        }

        #region No generic persistent operations.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="instance"></param>
        /// <exception cref="BusinessPersistentException"></exception>
        /// <exception cref="QueryArgumentException"></exception>
        /// <returns></returns>
        public static object MakePersistent
            (this ISessionContext sourceDAO, object instance)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (instance == null)
                throw new QueryArgumentException("The object to delete cannot be null.", "MakePersistent", "instance");

            try
            {
                session.SaveOrUpdate(instance);
                return instance;
            }
            catch (Exception ex)
            {
                throw new BusinessPersistentException(string.Format("Error on making persistent the given instance (type of <{0}>).", instance.GetType().Name), "MakePersistent", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="instance"></param>
        /// <param name="identifier"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="BusinessPersistentException"></exception>
        /// <returns></returns>
        public static object MakePersistent
            (this ISessionContext sourceDAO, object instance, object identifier)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (instance == null)
                throw new QueryArgumentException("The object to update cannot be null.", "MakePersistent", "instance");

            if (identifier == null)
                throw new QueryArgumentException("The identifier for updating ther given instance cannot be null.", "MakePersistent", "instance");

            try
            {
                session.Update(instance, identifier);
                return instance;
            }
            catch (Exception ex)
            {
                throw new BusinessPersistentException(string.Format("Error on making persistent the given instance (type of <{0}>).", instance.GetType().Name), "MakePersistent", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="instances"></param>
        /// <exception cref="BusinessPersistentException"></exception>
        /// <returns></returns>
        public static IEnumerable MakePersistent
            (this ISessionContext sourceDAO, IEnumerable instances)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            try
            {
                session.SetBatchSize(10);
                foreach (var instance in instances)
                {
                    sourceDAO.MakePersistent(instance);
                }
                return instances;
            }
            catch (Exception ex)
            {
                throw new BusinessPersistentException("Error on making persistent the given instances.", "MakePersistent", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="instance"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="BusinessPersistentException"></exception>
        public static void MakeTransient
            (this ISessionContext sourceDAO, object instance)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (instance == null)
                throw new QueryArgumentException("The object to delete cannot be null.", "MakeTransient", "instance");
            
            try
            {
                session.Delete(instance);
            }
            catch (Exception ex)
            {
                throw new BusinessPersistentException("Error on executing the delete query.", "MakeTransient", ex);
            }
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="criteria"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static ICollection FindAll
            (this ISessionContext sourceDAO, DetachedCriteria criteria)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;
            if (criteria == null)
                throw new QueryArgumentException("DetachedCriteria instance cannot be null.", "FindAll", "criteria");

            try
            {
                return criteria.GetExecutableCriteria(session).List();
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing the given DetachedCriteria query.", "FindAll", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static IPagedResult GetPagedResult
            (this ISessionContext sourceDAO, int startIndex, int pageSize, DetachedCriteria criteria)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (criteria == null)
                throw new QueryArgumentException("The criteria instance to execute cannot be null.", "GetPagedResult", "criteria");

            try
            {
                return new NhPagedResult(startIndex, pageSize, criteria.GetExecutableCriteria(session));
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing the paging criteria query.", "GetPagedResult", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="classType"></param>
        /// <exception cref="BusinessLayerException"></exception>
        /// <returns></returns>
        public static IClassMetadata GetPersistentClassInfo
            (this ISessionContext sourceDAO, Type classType)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (classType == null)
                throw new BusinessLayerException("The classType object cannot be null.");

            return session.SessionFactory.GetClassMetadata(classType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private static long RowCount(ICriteria criteria)
        {
            return CriteriaTransformer.Clone(criteria)
                                      .SetProjection(Projections.RowCountInt64())
                                      .UniqueResult<long>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static long RowCount
            (this ISessionContext sourceDAO, DetachedCriteria criteria)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;
            if (criteria == null)
                throw new QueryArgumentException("Criteria instance to execute cannot be null.", "RowCount", "criteria");
            try
            {
                return RowCount(criteria.GetExecutableCriteria(session));
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing the given detached criteria.", "RowCount", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static long RowCount<TEntity>
            (this ISessionContext sourceDAO, QueryOver<TEntity> query)
            where TEntity : class
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;
            if (query == null)
                throw new QueryArgumentException("Query instance to execute cannot be null.", "RowCount", "query");
            try
            {
                return RowCount(query.GetExecutableQueryOver(session).UnderlyingCriteria);
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing the given query over.", "RowCount", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> TransformResult<TEntity, TResult>
            (this ISessionContext sourceDAO, QueryOver<TEntity> query)
            where TEntity : class
        {
            return sourceDAO.TransformResult<TEntity, TResult>(Transformers.AliasToBean<TResult>(), query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="query"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static IEnumerable<object[]> ToProjectOver<TEntity>
            (this ISessionContext sourceDAO, QueryOver<TEntity> query)
            where TEntity : class
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;
            if (query == null)
                throw new QueryArgumentException("QueryOver instance to execute cannot be null.", "ToProjectOver", "query");
            try
            {
                return sourceDAO.ToProjectOver(query.GetExecutableQueryOver(session).UnderlyingCriteria);
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing the given projection query.", "ToProjectOver", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> TransformFutureResult<TEntity, TResult>
            (this ISessionContext sourceDAO, QueryOver<TEntity> query)
            where TEntity : class
        {
            return sourceDAO.TransformFutureResult<TEntity, TResult>(Transformers.AliasToBean<TResult>(), query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="query"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static IEnumerable<object[]> ToProjectOverFuture<TEntity>
            (this ISessionContext sourceDAO, QueryOver<TEntity> query)
            where TEntity : class
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;
            if (query == null)
                throw new QueryArgumentException("QueryOver instance to execute cannot be null.", "ToProjectOverFuture", "query");
            try
            {
                return sourceDAO.ToProjectOverFuture(query.GetExecutableQueryOver(session).UnderlyingCriteria);
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing the given projection query.", "ToProjectOver", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> TransformResult<TResult>
            (this ISessionContext sourceDAO, DetachedCriteria criteria)
        {
            return sourceDAO.TransformResult<TResult>(Transformers.AliasToBean<TResult>(), criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="criteria"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static IEnumerable<object[]> ToProjectOver
            (this ISessionContext sourceDAO, DetachedCriteria criteria)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;
            if (criteria == null)
                throw new QueryArgumentException("Criteria instance to execute cannot be null.", "ToProjectOver", "criteria");

            try
            {
                return sourceDAO.ToProjectOver(criteria.GetExecutableCriteria(session));
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing the given projection query.", "ToProjectOver", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> TransformFutureResult<TResult>
            (this ISessionContext sourceDAO, DetachedCriteria criteria)
        {
            return sourceDAO.TransformFutureResult<TResult>(Transformers.AliasToBean<TResult>(), criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="criteria"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static IEnumerable<object[]> ToProjectOverFuture
            (this ISessionContext sourceDAO, DetachedCriteria criteria)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;
            if (criteria == null)
                throw new QueryArgumentException("Criteria instance to execute cannot be null.", "ToProjectOverFuture", "criteria");

            try
            {
                return sourceDAO.ToProjectOverFuture(criteria.GetExecutableCriteria(session));
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing the given projection query.", "ToProjectOverFuture", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="transformer"></param>
        /// <param name="query"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static IEnumerable<TResult> TransformResult<TEntity, TResult>
            (this ISessionContext sourceDAO, IResultTransformer transformer, QueryOver<TEntity> query)
            where TEntity : class
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (query == null)
                throw new QueryArgumentException("Query to execute cannot be null.", "TransformResult", "query");

            if (transformer == null)
                throw new QueryArgumentException("Transformer cannot be null.", "TransformResult", "transformer");

            try
            {
                return sourceDAO.TransformResult<TResult>(transformer, query.GetExecutableQueryOver(session).UnderlyingCriteria); // verificare se solleva un'eccezione.;
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing transformer method with the given QueryOver instance.", "TransformResult", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="transformer"></param>
        /// <param name="query"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static IEnumerable<TResult> TransformFutureResult<TEntity, TResult>
            (this ISessionContext sourceDAO, IResultTransformer transformer, QueryOver<TEntity> query)
            where TEntity : class
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (query == null)
                throw new QueryArgumentException("Query to execute cannot be null.", "TransformFutureResult", "query");

            if (transformer == null)
                throw new QueryArgumentException("Transformer cannot be null.", "TransformFutureResult", "transformer");

            try
            {
                return sourceDAO.TransformFutureResult<TResult>(transformer, query.GetExecutableQueryOver(session).UnderlyingCriteria); // verificare se solleva un'eccezione.;
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing transformer method with the given QueryOver instance.", "TransformResult", ex);
            }
        }

        /// <summary>
        /// Set a strategy for handling the query results. This transforms the query result into specific collection typed.
        /// </summary>
        /// <typeparam name="TResult">The type of result</typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="transformer">The tranformer which converts the result query into collection result typed</param>
        /// <param name="criteria">The given detached criteria to invoke to get result to transforming.</param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static IEnumerable<TResult> TransformResult<TResult>
            (this ISessionContext sourceDAO, IResultTransformer transformer, DetachedCriteria criteria)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (criteria == null)
                throw new QueryArgumentException("Query to execute cannot be null.", "TransformResult", "criteria");

            if (transformer == null)
                throw new QueryArgumentException("Transformer cannot be null.", "TransformResult", "transformer");

            try
            {
                return sourceDAO.TransformResult<TResult>(transformer, criteria.GetExecutableCriteria(session));
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing transformer method with the given DetachedCriteria instance.", "TransformResult", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="transformer"></param>
        /// <param name="criteria"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="ExecutionQueryException"></exception>
        /// <returns></returns>
        public static IEnumerable<TResult> TransformFutureResult<TResult>
            (this ISessionContext sourceDAO, IResultTransformer transformer, DetachedCriteria criteria)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (criteria == null)
                throw new QueryArgumentException("Query to execute cannot be null.", "TransformFutureResult", "criteria");

            if (transformer == null)
                throw new QueryArgumentException("Transformer cannot be null.", "TransformFutureResult", "transformer");

            try
            {
                return sourceDAO.TransformFutureResult<TResult>(transformer, criteria.GetExecutableCriteria(session));
            }
            catch (Exception ex)
            {
                throw new ExecutionQueryException("Error on executing transformer method with the given DetachedCriteria instance.", "TransformResult", ex);
            }
        }

        /// <summary>
        /// Set a strategy for handling the query results. This determines the "shape" of the query result set.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="transformer"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private static IEnumerable<TResult> TransformResult<TResult>
            (this ISessionContext sourceDAO, IResultTransformer transformer , ICriteria criteria)
        {
            return criteria.SetResultTransformer(transformer).List<TResult>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="transformer"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private static IEnumerable<TResult> TransformFutureResult<TResult>
            (this ISessionContext sourceDAO, IResultTransformer transformer, ICriteria criteria)
        {
            return criteria.SetResultTransformer(transformer).Future<TResult>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private static IEnumerable<object[]> ToProjectOver
            (this ISessionContext sourceDAO, ICriteria criteria)
        {
            return criteria.List<object[]>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private static IEnumerable<object[]> ToProjectOverFuture
            (this ISessionContext sourceDAO, ICriteria criteria)
        {
            return criteria.Future<object[]>();
        }

        /// <summary>
        /// Get an instance of IQuery for a named query defined into mapping file.
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public static IQuery GetNamedQuery
            (this ISessionContext sourceDAO , string queryName)
        {
            return sourceDAO.SessionInfo.CurrentSession.GetNamedQuery(queryName);
        }

        /// <summary>
        /// Make a NHibernate.ISQLQuery instance for the given SQL query string.
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="query"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="QueryFormatException"></exception>
        /// <returns></returns>
        public static ISQLQuery MakeSQLQuery
            (this ISessionContext sourceDAO, string query)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (string.IsNullOrEmpty(query))
                throw new QueryArgumentException("Native SQL query to make cannot be empty or null", "MakeSQLQuery", "query");

            try
            {
                return session.CreateSQLQuery(query);
            }
            catch (Exception ex)
            {
                throw new QueryFormatException(ex.Message, "MakeSQLQuery", ex);
            }
        }

        /// <summary>
        /// Make a IQuery instance for the given HQL string.
        /// </summary>
        /// <param name="sourceDAO"></param>
        /// <param name="query"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="QueryFormatException"></exception>
        /// <returns></returns>
        public static IQuery MakeHQLQuery
            (this ISessionContext sourceDAO, string query)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;
            
            if (string.IsNullOrEmpty(query))
                throw new QueryArgumentException("HQL query to make cannot be empty or null", "MakeHQLQuery", "query");
            
            try
            {
                return session.CreateQuery(query);
            }
            catch (Exception ex)
            {
                throw new QueryFormatException(ex.Message, "MakeHQLQuery", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceDAO"></param>
        /// <param name="instances"></param>
        /// <param name="filter"></param>
        /// <exception cref="QueryArgumentException"></exception>
        /// <exception cref="QueryFormatException"></exception>
        /// <returns></returns>
        public static IQuery MakeFilter<TEntity>
            (this ISessionContext sourceDAO, IEnumerable<TEntity> instances, string filter)
        {
            ISession session = sourceDAO.SessionInfo.CurrentSession;

            if (instances == null)
                throw new QueryArgumentException("Persistent collection cannot be null.", "MakeFilter", "instances");

            try
            {
                return session.CreateFilter(instances, filter);
            }
            catch (Exception ex)
            {
                throw new QueryFormatException("It's impossible to make a filter with the given persistent collection.", "MakeFilter", ex);
            }
        }
    }
}
