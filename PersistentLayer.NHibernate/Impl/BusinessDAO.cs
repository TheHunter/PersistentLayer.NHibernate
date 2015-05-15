using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// Class BusinessDAO.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    public class BusinessDAO<TEntity, TKey>
        : AbstractDAO, IEntityDAO<TEntity, TKey>
        where TEntity : class
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDAO" /> class.
        /// </summary>
        /// <param name="sessionProvider">The session provider.</param>
        public BusinessDAO(ISessionProvider sessionProvider)
            :base(sessionProvider)
        {
        }

        /// <summary>
        /// Finds instance by the given identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        public TEntity FindBy(TKey identifier)
        {
            return this.CurrentSession.FindBy<TEntity, TKey>(identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public TEntity FindBy(TKey identifier, LockMode mode)
        {
            return this.CurrentSession.FindBy<TEntity, TKey>(identifier, mode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll()
        {
            return this.CurrentSession.FindAll<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return this.CurrentSession.FindAll(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheable"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(bool cacheable)
        {
            return this.CurrentSession.FindAll<TEntity>(cacheable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheRegion"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(string cacheRegion)
        {
            return this.CurrentSession.FindAll<TEntity>(cacheRegion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fetchSize"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(int fetchSize)
        {
            return this.CurrentSession.FindAll<TEntity>(fetchSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(DetachedCriteria criteria)
        {
            return this.CurrentSession.FindAll<TEntity>(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll(QueryOver<TEntity> query)
        {
            return this.CurrentSession.FindAll(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAllFuture(DetachedCriteria criteria)
        {
            return this.CurrentSession.FindAllFuture<TEntity>(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAllFuture(QueryOver<TEntity> query)
        {
            return this.CurrentSession.FindAllFuture(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFutureValue"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IFutureValue<TFutureValue> GetFutureValue<TFutureValue>(DetachedCriteria criteria)
        {
            return this.CurrentSession.GetFutureValue<TFutureValue>(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFutureValue"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public IFutureValue<TFutureValue> GetFutureValue<TFutureValue>(QueryOver<TEntity> query)
        {
            return this.CurrentSession.GetFutureValue<TEntity, TFutureValue>(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool Exists(TKey identifier)
        {
            return this.CurrentSession.Exists<TEntity, TKey>(identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifiers"></param>
        /// <returns></returns>
        public bool Exists(IEnumerable<TKey> identifiers)
        {
            return this.CurrentSession.Exists<TEntity, TKey>(identifiers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return this.CurrentSession.Exists(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public bool Exists(DetachedCriteria criteria)
        {
            return this.CurrentSession.Exists(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool Exists(QueryOver<TEntity> query)
        {
            return this.CurrentSession.Exists(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity MakePersistent(TEntity entity)
        {
            return this.CurrentSession.MakePersistent(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public TEntity MakePersistent(TEntity entity, TKey identifier)
        {
            return this.CurrentSession.MakePersistent(entity, identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> MakePersistent(IEnumerable<TEntity> entities)
        {
            return this.CurrentSession.MakePersistent(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void MakeTransient(TEntity entity)
        {
            this.CurrentSession.MakeTransient(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public void MakeTransient(IEnumerable<TEntity> entities)
        {
            this.CurrentSession.MakeTransient(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity RefreshState(TEntity entity)
        {
            return this.CurrentSession.RefreshState(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> RefreshState(IEnumerable<TEntity> entities)
        {
            return this.CurrentSession.RefreshState(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity UniqueResult(Expression<Func<TEntity, bool>> predicate)
        {
            return this.CurrentSession.UniqueResult(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TEntity UniqueResult(DetachedCriteria criteria)
        {
            return this.CurrentSession.UniqueResult<TEntity>(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TEntity UniqueResult(QueryOver<TEntity> criteria)
        {
            return this.CurrentSession.UniqueResult(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <returns></returns>
        public TResult ExecuteExpression<TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr)
        {
            return this.CurrentSession.ExecuteExpression(queryExpr, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public TResult ExecuteExpression<TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr, CacheMode mode)
        {
            return this.CurrentSession.ExecuteExpression(queryExpr, mode, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public TResult ExecuteExpression<TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr, string region)
        {
            return this.CurrentSession.ExecuteExpression(queryExpr, null, region);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <param name="mode"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public TResult ExecuteExpression<TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr, CacheMode mode, string region)
        {
            return this.CurrentSession.ExecuteExpression(queryExpr, mode, region);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable()
        {
            return this.CurrentSession.ToIQueryable<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable(CacheMode mode)
        {
            return this.CurrentSession.ToIQueryable<TEntity>(mode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable(string region)
        {
            return this.CurrentSession.ToIQueryable<TEntity>(region);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable(CacheMode mode, string region)
        {
            return this.CurrentSession.ToIQueryable<TEntity>(mode, region);
        }
    }
}
