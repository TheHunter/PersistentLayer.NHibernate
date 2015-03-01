using System.Linq;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Interface INhPagedDAO
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    public interface INhPagedDAO<TEntity, TKey>
        : IEntityDAO<TEntity, TKey>, IPagedDAO<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>
        /// Gets the paged result.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IPagedResult&lt;TEntity&gt;.</returns>
        IPagedResult<TEntity> GetPagedResult(int startIndex, int pageSize, DetachedCriteria criteria);

        /// <summary>
        /// Gets the paged result.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="query">The query.</param>
        /// <returns>IPagedResult&lt;TEntity&gt;.</returns>
        IPagedResult<TEntity> GetPagedResult(int startIndex, int pageSize, QueryOver<TEntity> query);

        /// <summary>
        /// Gets the index paged result.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IPagedResult&lt;TEntity&gt;.</returns>
        IPagedResult<TEntity> GetIndexPagedResult(int pageIndex, int pageSize, DetachedCriteria criteria);

        /// <summary>
        /// Gets the index paged result.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="query">The query.</param>
        /// <returns>IPagedResult&lt;TEntity&gt;.</returns>
        IPagedResult<TEntity> GetIndexPagedResult(int pageIndex, int pageSize, QueryOver<TEntity> query);

    }

    /// <summary>
    /// Interface INhPagedDAO
    /// </summary>
    public interface INhPagedDAO
        : IDomainDAO, IPagedDAO
    {
        /// <summary>
        /// Gets the paged result.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="startIndex">The start index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IPagedResult&lt;TEntity&gt;.</returns>
        IPagedResult<TEntity> GetPagedResult<TEntity>(int startIndex, int pageSize, DetachedCriteria criteria)
             where TEntity : class;

        /// <summary>
        /// Gets the paged result.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="startIndex">The start index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="query">The query.</param>
        /// <returns>IPagedResult&lt;TEntity&gt;.</returns>
        IPagedResult<TEntity> GetPagedResult<TEntity>(int startIndex, int pageSize, QueryOver<TEntity> query)
             where TEntity : class;

        /// <summary>
        /// Gets the index paged result.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IPagedResult&lt;TEntity&gt;.</returns>
        IPagedResult<TEntity> GetIndexPagedResult<TEntity>(int pageIndex, int pageSize, DetachedCriteria criteria)
            where TEntity : class;

        /// <summary>
        /// Gets the index paged result.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="query">The query.</param>
        /// <returns>IPagedResult&lt;TEntity&gt;.</returns>
        IPagedResult<TEntity> GetIndexPagedResult<TEntity>(int pageIndex, int pageSize, QueryOver<TEntity> query)
            where TEntity : class;

    }
}
