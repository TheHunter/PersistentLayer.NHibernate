using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class EnterpriseDAO
        : AbstractDAO, IDomainDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionProvider"></param>
        public EnterpriseDAO(ISessionProvider sessionProvider)
            :base(sessionProvider)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public TEntity FindBy<TEntity, TKey>(TKey identifier) where TEntity : class
        {
            return this.CurrentSession.FindBy<TEntity, TKey>(identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public TEntity FindBy<TEntity, TKey>(TKey identifier, LockMode mode) where TEntity : class
        {
            return this.CurrentSession.FindBy<TEntity, TKey>(identifier, mode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>() where TEntity : class
        {
            return this.CurrentSession.FindAll<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return this.CurrentSession.FindAll(where);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cacheable"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(bool cacheable) where TEntity : class
        {
            return this.CurrentSession.FindAll<TEntity>(cacheable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cacheRegion"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(string cacheRegion) where TEntity : class
        {
            return this.CurrentSession.FindAll<TEntity>(cacheRegion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fetchSize"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(int fetchSize) where TEntity : class
        {
            return this.CurrentSession.FindAll<TEntity>(fetchSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(DetachedCriteria criteria) where TEntity : class
        {
            return this.CurrentSession.FindAll<TEntity>(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(QueryOver<TEntity> query) where TEntity : class
        {
            return this.CurrentSession.FindAll(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAllFuture<TEntity>(DetachedCriteria criteria) where TEntity : class
        {
            return this.CurrentSession.FindAllFuture<TEntity>(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAllFuture<TEntity>(QueryOver<TEntity> query) where TEntity : class
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
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TFutureValue"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public IFutureValue<TFutureValue> GetFutureValue<TEntity, TFutureValue>(QueryOver<TEntity> query) where TEntity : class
        {
            return this.CurrentSession.GetFutureValue<TEntity, TFutureValue>(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool Exists<TEntity, TKey>(TKey identifier) where TEntity : class
        {
            return this.CurrentSession.Exists<TEntity, TKey>(identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="identifiers"></param>
        /// <returns></returns>
        public bool Exists<TEntity, TKey>(IEnumerable<TKey> identifiers) where TEntity : class
        {
            return this.CurrentSession.Exists<TEntity, TKey>(identifiers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
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
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool Exists<TEntity>(QueryOver<TEntity> query) where TEntity : class
        {
            return this.CurrentSession.Exists(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity MakePersistent<TEntity>(TEntity entity) where TEntity : class
        {
            return this.CurrentSession.MakePersistent(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public TEntity MakePersistent<TEntity, TKey>(TEntity entity, TKey identifier) where TEntity : class
        {
            return this.CurrentSession.MakePersistent(entity, identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> MakePersistent<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            return this.CurrentSession.MakePersistent(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void MakeTransient<TEntity>(TEntity entity) where TEntity : class
        {
            this.CurrentSession.MakeTransient(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public void MakeTransient<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            this.CurrentSession.MakeTransient(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity RefreshState<TEntity>(TEntity entity) where TEntity : class
        {
            return this.CurrentSession.RefreshState(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> RefreshState<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            return this.CurrentSession.RefreshState(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity UniqueResult<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return this.CurrentSession.UniqueResult(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TEntity UniqueResult<TEntity>(DetachedCriteria criteria) where TEntity : class
        {
            return this.CurrentSession.UniqueResult<TEntity>(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TEntity UniqueResult<TEntity>(QueryOver<TEntity> criteria) where TEntity : class
        {
            return this.CurrentSession.UniqueResult(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <returns></returns>
        public TResult ExecuteExpression<TEntity, TResult>(Expression<Func<IEnumerable<TEntity>, TResult>> queryExpr) where TEntity : class
        {
            return this.CurrentSession.ExecuteExpression(queryExpr, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public TResult ExecuteExpression<TEntity, TResult>(Expression<Func<IEnumerable<TEntity>, TResult>> queryExpr, CacheMode mode) where TEntity : class
        {
            return this.CurrentSession.ExecuteExpression(queryExpr, mode, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public TResult ExecuteExpression<TEntity, TResult>(Expression<Func<IEnumerable<TEntity>, TResult>> queryExpr, string region) where TEntity : class
        {
            return this.CurrentSession.ExecuteExpression(queryExpr, null, region);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <param name="mode"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public TResult ExecuteExpression<TEntity, TResult>(Expression<Func<IEnumerable<TEntity>, TResult>> queryExpr, CacheMode mode, string region) where TEntity : class
        {
            return this.CurrentSession.ExecuteExpression(queryExpr, mode, region);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable<TEntity>() where TEntity : class
        {
            return this.CurrentSession.ToIQueryable<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="mode"></param>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable<TEntity>(CacheMode mode) where TEntity : class
        {
            return this.CurrentSession.ToIQueryable<TEntity>(mode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="region"></param>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable<TEntity>(string region) where TEntity : class
        {
            return this.CurrentSession.ToIQueryable<TEntity>(region);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="mode"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable<TEntity>(CacheMode mode, string region) where TEntity : class
        {
            return this.CurrentSession.ToIQueryable<TEntity>(mode, region);
        }
    }
}
