using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Interface INhQueryableDAO
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    public interface INhQueryableDAO<TEntity, TKey>
        : IQueryableDAO<TEntity, TKey>
        where TEntity : class
    {
        #region IQueryable Interface
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use ExecuteExpression method in order to use Linq Expressions.")]
        IQueryable<TEntity> ToIQueryable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [Obsolete("Use ExecuteExpression method in order to use Linq Expressions.")]
        IQueryable<TEntity> ToIQueryable(CacheMode mode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        [Obsolete("Use ExecuteExpression method in order to use Linq Expressions.")]
        IQueryable<TEntity> ToIQueryable(string region);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        [Obsolete("Use ExecuteExpression method in order to use Linq Expressions.")]
        IQueryable<TEntity> ToIQueryable(CacheMode mode, string region);
        #endregion

        #region Advanced query expressions.

        /// <summary>
        /// Executes the expression.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="queryExpr">The query expr.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>TResult.</returns>
        TResult ExecuteExpression<TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr, CacheMode mode);

        /// <summary>
        /// Executes the expression.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="queryExpr">The query expr.</param>
        /// <param name="region">The region.</param>
        /// <returns>TResult.</returns>
        TResult ExecuteExpression<TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr, string region);

        /// <summary>
        /// Executes the expression.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="queryExpr">The query expr.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="region">The region.</param>
        /// <returns>TResult.</returns>
        TResult ExecuteExpression<TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr, CacheMode mode,
                                           string region);
        #endregion
        

        /// <summary>
        /// Finds the object which matches with the given key.
        /// </summary>
        /// <param name="identifier">The key of instance to get.</param>
        /// <param name="mode">The type of locking for the instance to get, this argument can be null.</param>
        /// <returns>return an instance related to the calling type object.</returns>
        TEntity FindBy(TKey identifier, LockMode mode);

        /// <summary>
        /// gets all instances from data source, and cache the query.
        /// </summary>
        /// <param name="cacheable">specify if cache will be switch on.</param>
        /// <returns>a set of instances from database.</returns>
        IEnumerable<TEntity> FindAll(bool cacheable);

        /// <summary>
        /// Finds all instances associated if It exists the given cache.
        /// </summary>
        /// <param name="cacheRegion">The cache region.</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        IEnumerable<TEntity> FindAll(string cacheRegion);

        /// <summary>
        /// Finds all isntances using the given fetch size.
        /// </summary>
        /// <param name="fetchSize">Size of the fetch.</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        IEnumerable<TEntity> FindAll(int fetchSize);

        /// <summary>
        /// Finds all instances using the given criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        IEnumerable<TEntity> FindAll(DetachedCriteria criteria);

        /// <summary>
        /// Finds all instances using the given query over.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        IEnumerable<TEntity> FindAll(QueryOver<TEntity> query);

        #region Future section

        /// <summary>
        /// Finds all future instances using the given criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        IEnumerable<TEntity> FindAllFuture(DetachedCriteria criteria);

        /// <summary>
        /// Finds all future instances using the given query over criteria.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        IEnumerable<TEntity> FindAllFuture(QueryOver<TEntity> query);

        /// <summary>
        /// Gets the future value.
        /// </summary>
        /// <typeparam name="TFutureValue">The type of the t future value.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IFutureValue&lt;TFutureValue&gt;.</returns>
        IFutureValue<TFutureValue> GetFutureValue<TFutureValue>(DetachedCriteria criteria);

        /// <summary>
        /// Gets the future value.
        /// </summary>
        /// <typeparam name="TFutureValue">The type of the t future value.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>IFutureValue&lt;TFutureValue&gt;.</returns>
        IFutureValue<TFutureValue> GetFutureValue<TFutureValue>(QueryOver<TEntity> query);

        #endregion

        /// <summary>
        /// Exists instances using the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Exists(DetachedCriteria criteria);

        /// <summary>
        /// Exists instances the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Exists(QueryOver<TEntity> query);

        /// <summary>
        /// Refreshes the state.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TEntity.</returns>
        TEntity RefreshState(TEntity entity);

        /// <summary>
        /// Refreshes the state.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        IEnumerable<TEntity> RefreshState(IEnumerable<TEntity> entities);

        /// <summary>
        /// Uniques the result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>TEntity.</returns>
        TEntity UniqueResult(DetachedCriteria criteria);

        /// <summary>
        /// Uniques the result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>TEntity.</returns>
        TEntity UniqueResult(QueryOver<TEntity> criteria);

    }

    /// <summary>
    /// Interface INhQueryableDAO
    /// </summary>
    public interface INhQueryableDAO
        : IQueryableDAO
    {
        #region IQueryable Interface, They must be deprecated
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use ExecuteExpression method in order to use Linq Expressions.")]
        IQueryable<TEntity> ToIQueryable<TEntity>() where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [Obsolete("Use ExecuteExpression method in order to use Linq Expressions.")]
        IQueryable<TEntity> ToIQueryable<TEntity>(CacheMode mode) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        [Obsolete("Use ExecuteExpression method in order to use Linq Expressions.")]
        IQueryable<TEntity> ToIQueryable<TEntity>(string region) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="region"></param>
        [Obsolete("Use ExecuteExpression method in order to use Linq Expressions.")]
        IQueryable<TEntity> ToIQueryable<TEntity>(CacheMode mode, string region) where TEntity : class;
        #endregion

        #region Advanced query expressions.
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TResult ExecuteExpression<TEntity, TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr, CacheMode mode) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        TResult ExecuteExpression<TEntity, TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr, string region) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryExpr"></param>
        /// <param name="mode"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        TResult ExecuteExpression<TEntity, TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr, CacheMode mode,
                                           string region) where TEntity : class;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TEntity FindBy<TEntity, TKey>(TKey identifier, LockMode mode) where TEntity : class;

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
        IFutureValue<TFutureValue> GetFutureValue<TEntity, TFutureValue>(QueryOver<TEntity> query) where TEntity : class;

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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TEntity UniqueResult<TEntity>(DetachedCriteria criteria) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TEntity UniqueResult<TEntity>(QueryOver<TEntity> criteria) where TEntity : class;
    }
}
