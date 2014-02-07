using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainDAO
        : INhQueryableDAO, IPersisterDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TEntity FindBy<TEntity, TKey>(TKey id, LockMode mode) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cacheable"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(bool cacheable) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cacheRegion"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(string cacheRegion) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fetchSize"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(int fetchSize) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(DetachedCriteria criteria) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAll<TEntity>(QueryOver<TEntity> query) where TEntity : class;

        #region future section
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAllFuture<TEntity>(DetachedCriteria criteria) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindAllFuture<TEntity>(QueryOver<TEntity> query) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="FutureValue"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IFutureValue<FutureValue> GetFutureValue<FutureValue>(DetachedCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="FutureValue"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IFutureValue<FutureValue> GetFutureValue<TEntity, FutureValue>(QueryOver<TEntity> query) where TEntity : class;

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
        bool Exists<TEntity>(QueryOver<TEntity> query) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity RefreshState<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        IEnumerable<TEntity> RefreshState<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    }
}
