using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRootEntity"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class EnterpriseRootDAO<TRootEntity, TEntity>
        : AbstractDAO, INhRootPagedDAO<TRootEntity, TEntity>
        where TRootEntity : class
        where TEntity : class, TRootEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionProvider"></param>
        public EnterpriseRootDAO(ISessionProvider sessionProvider)
            :base (sessionProvider)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool Exists(object identifier)
        {
            return this.CurrentSession.Exists<TEntity, object>(identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifiers"></param>
        /// <returns></returns>
        public bool Exists(ICollection identifiers)
        {
            return this.CurrentSession.Exists<TEntity>(identifiers);
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
        /// <param name="identifier"></param>
        /// <returns></returns>
        public TEntity FindBy(object identifier)
        {
            return this.CurrentSession.FindBy<TEntity, object>(identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public TEntity FindBy(object identifier, LockMode mode)
        {
            return this.CurrentSession.FindBy<TEntity, object>(identifier, mode);
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
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetIndexPagedResult(int pageIndex, int pageSize, DetachedCriteria criteria)
        {
            return this.CurrentSession.GetIndexPagedResult<TEntity>(pageIndex, pageSize, criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetIndexPagedResult(int pageIndex, int pageSize, QueryOver<TEntity> query)
        {
            return this.CurrentSession.GetIndexPagedResult(pageIndex, pageSize, query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetPagedResult(int startIndex, int pageSize, DetachedCriteria criteria)
        {
            return this.CurrentSession.GetPagedResult<TEntity>(startIndex, pageSize, criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetPagedResult(int startIndex, int pageSize, QueryOver<TEntity> query)
        {
            return this.CurrentSession.GetPagedResult(startIndex, pageSize, query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetPagedResult(int startIndex, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            var IQuerable = this.ToIQueryable()
                                .Where(predicate);
            return this.CurrentSession.GetPagedResult(startIndex, pageSize, IQuerable);
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
        public TEntity MakePersistent(TEntity entity, object identifier)
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

        ///// <summary>
        ///// 
        ///// </summary>
        //SessionInfo ISessionContext.SessionInfo
        //{
        //    get { return SessionInfo; }
        //}

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


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRootEntity"></typeparam>
    public class EnterpriseRootDAO<TRootEntity>
        : AbstractDAO, INhRootPagedDAO<TRootEntity>
        where TRootEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionProvider"></param>
        public EnterpriseRootDAO(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool Exists<TEntity>(object identifier) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.Exists<TEntity, object>(identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="identifiers"></param>
        /// <returns></returns>
        public bool Exists<TEntity>(ICollection identifiers) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.Exists<TEntity>(identifiers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, TRootEntity
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
        public bool Exists<TEntity>(QueryOver<TEntity> query) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.Exists(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public TEntity FindBy<TEntity>(object identifier) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.FindBy<TEntity, object>(identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public TEntity FindBy<TEntity>(object identifier, LockMode mode) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.FindBy<TEntity, object>(identifier, mode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>() where TEntity : class, TRootEntity
        {
            return this.CurrentSession.FindAll<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.FindAll(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cacheable"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(bool cacheable) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.FindAll<TEntity>(cacheable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cacheRegion"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(string cacheRegion) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.FindAll<TEntity>(cacheRegion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fetchSize"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(int fetchSize) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.FindAll<TEntity>(fetchSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(DetachedCriteria criteria) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.FindAll<TEntity>(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAll<TEntity>(QueryOver<TEntity> query) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.FindAll(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAllFuture<TEntity>(DetachedCriteria criteria) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.FindAllFuture<TEntity>(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindAllFuture<TEntity>(QueryOver<TEntity> query) where TEntity : class, TRootEntity
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
        public IFutureValue<TFutureValue> GetFutureValue<TEntity, TFutureValue>(QueryOver<TEntity> query) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.GetFutureValue<TEntity, TFutureValue>(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetIndexPagedResult<TEntity>(int pageIndex, int pageSize, DetachedCriteria criteria) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.GetIndexPagedResult<TEntity>(pageIndex, pageSize, criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetIndexPagedResult<TEntity>(int pageIndex, int pageSize, QueryOver<TEntity> query) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.GetIndexPagedResult(pageIndex, pageSize, query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetPagedResult<TEntity>(int startIndex, int pageSize, DetachedCriteria criteria) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.GetPagedResult<TEntity>(startIndex, pageSize, criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetPagedResult<TEntity>(int startIndex, int pageSize, QueryOver<TEntity> query) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.GetPagedResult(startIndex, pageSize, query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetPagedResult<TEntity>(int startIndex, int pageSize, Expression<Func<TEntity, bool>> predicate) where TEntity : class, TRootEntity
        {
            var IQuerable = this.ToIQueryable<TEntity>()
                                .Where(predicate);
            return this.CurrentSession.GetPagedResult(startIndex, pageSize, IQuerable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity MakePersistent<TEntity>(TEntity entity) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.MakePersistent(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public TEntity MakePersistent<TEntity>(TEntity entity, object identifier) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.MakePersistent<TEntity, object>(entity, identifier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> MakePersistent<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.MakePersistent(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void MakeTransient<TEntity>(TEntity entity) where TEntity : class, TRootEntity
        {
            this.CurrentSession.MakeTransient(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public void MakeTransient<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, TRootEntity
        {
            this.CurrentSession.MakeTransient(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity RefreshState<TEntity>(TEntity entity) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.RefreshState(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> RefreshState<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.RefreshState(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity UniqueResult<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.UniqueResult(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TEntity UniqueResult<TEntity>(DetachedCriteria criteria) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.UniqueResult<TEntity>(criteria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TEntity UniqueResult<TEntity>(QueryOver<TEntity> criteria) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.UniqueResult(criteria);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //SessionInfo ISessionContext.SessionInfo
        //{
        //    get { return SessionInfo; }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable<TEntity>() where TEntity : class, TRootEntity
        {
            return this.CurrentSession.ToIQueryable<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="mode"></param>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable<TEntity>(CacheMode mode) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.ToIQueryable<TEntity>(mode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="region"></param>
        /// <returns></returns>
        public IQueryable<TEntity> ToIQueryable<TEntity>(string region) where TEntity : class, TRootEntity
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
        public IQueryable<TEntity> ToIQueryable<TEntity>(CacheMode mode, string region) where TEntity : class, TRootEntity
        {
            return this.CurrentSession.ToIQueryable<TEntity>(mode, region);
        }
    }
}
