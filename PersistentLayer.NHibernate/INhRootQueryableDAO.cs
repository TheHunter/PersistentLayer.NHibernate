using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public interface INhRootQueryableDAO<in TRootEntity, TEntity>
        : IRootQueryableDAO<TRootEntity, TEntity>
        where TRootEntity : class
        where TEntity : class, TRootEntity
    {
        #region IQueryable instance
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable(CacheMode mode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable(string region);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable(CacheMode mode, string region);
        #endregion


        /// <summary>
        /// Finds the object which matches with the given key.
        /// </summary>
        /// <param name="identifier">The key of instance to get.</param>
        /// <param name="mode">The type of locking for the instance to get, this argument can be null.</param>
        /// <returns>return an instance related to the calling type object.</returns>
        TEntity FindBy(object identifier, LockMode mode);

        /// <summary>
        /// gets all instances from data source, and cache the query.
        /// </summary>
        /// <param name="cacheable">specify if cache will be switch on.</param>
        /// <returns>a set of instances from database.</returns>
        IEnumerable<TEntity> FindAll(bool cacheable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheRegion"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll(string cacheRegion);

        /// <summary>
        /// sets the pre loaded of istances from the database.
        /// </summary>
        /// <param name="fetchSize"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll(int fetchSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll(QueryOver<TEntity> query);

        #region Future section
        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAllFuture(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAllFuture(QueryOver<TEntity> query);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFutureValue"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IFutureValue<TFutureValue> GetFutureValue<TFutureValue>(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFutureValue"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IFutureValue<TFutureValue> GetFutureValue<TFutureValue>(QueryOver<TEntity> query);

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        bool Exists(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        bool Exists(QueryOver<TEntity> query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity RefreshState(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IEnumerable<TEntity> RefreshState(IEnumerable<TEntity> entities);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TEntity UniqueResult(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TEntity UniqueResult(QueryOver<TEntity> criteria);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface INhRootQueryableDAO<in TRootEntity>
        : IRootQueryableDAO<TRootEntity>
        where TRootEntity : class
    {
        #region IQueryable instance
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable<TEntity>() where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable<TEntity>(CacheMode mode) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable<TEntity>(string region) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable<TEntity>(CacheMode mode, string region) where TEntity : class, TRootEntity;
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TEntity FindBy<TEntity>(object identifier, LockMode mode) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cacheable"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(bool cacheable) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cacheRegion"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(string cacheRegion) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fetchSize"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(int fetchSize) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(DetachedCriteria criteria) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(QueryOver<TEntity> query) where TEntity : class, TRootEntity;

        #region future section
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAllFuture<TEntity>(DetachedCriteria criteria) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAllFuture<TEntity>(QueryOver<TEntity> query) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFutureValue"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IFutureValue<TFutureValue> GetFutureValue<TFutureValue>(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TFutureValue"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IFutureValue<TFutureValue> GetFutureValue<TEntity, TFutureValue>(QueryOver<TEntity> query) where TEntity : class, TRootEntity;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        bool Exists(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        bool Exists<TEntity>(QueryOver<TEntity> query) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity RefreshState<TEntity>(TEntity entity) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        IEnumerable<TEntity> RefreshState<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TEntity UniqueResult<TEntity>(DetachedCriteria criteria) where TEntity : class, TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TEntity UniqueResult<TEntity>(QueryOver<TEntity> criteria) where TEntity : class, TRootEntity;
    }

}
