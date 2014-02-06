using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BusinessPagedDAO<TEntity, TKey>
        : BusinessDAO<TEntity, TKey>, INhPagedDAO<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionProvider"></param>
        public BusinessPagedDAO(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
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

        //public IPagedResult<TEntity> GetPagedResult(int startIndex, int pageSize, IQueryable<TEntity> query)
        //{
        //    return this.CurrentSession.GetPagedResult<TEntity>(startIndex, pageSize, query);
        //}

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

        
        //public IPagedResult<TEntity> GetIndexPagedResult(int pageIndex, int pageSize, IQueryable<TEntity> query)
        //{
        //    return this.CurrentSession.GetIndexPagedResult<TEntity>(pageIndex, pageSize, query);
        //}

    }
}
