using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class EnterprisePagedDAO
        : EnterpriseDAO, INhPagedDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionProvider"></param>
        public EnterprisePagedDAO(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetPagedResult<TEntity>(int startIndex, int pageSize, DetachedCriteria criteria)
            where TEntity : class
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
        public IPagedResult<TEntity> GetPagedResult<TEntity>(int startIndex, int pageSize, QueryOver<TEntity> query)
            where TEntity : class
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
        public IPagedResult<TEntity> GetPagedResult<TEntity>(int startIndex, int pageSize, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var IQuerable = this.ToIQueryable<TEntity>()
                                .Where(predicate);
            return this.CurrentSession.GetPagedResult(startIndex, pageSize, IQuerable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IPagedResult<TEntity> GetIndexPagedResult<TEntity>(int pageIndex, int pageSize, DetachedCriteria criteria)
            where TEntity : class
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
        public IPagedResult<TEntity> GetIndexPagedResult<TEntity>(int pageIndex, int pageSize, QueryOver<TEntity> query)
            where TEntity : class
        {
            return this.CurrentSession.GetIndexPagedResult(pageIndex, pageSize, query);
        }

        
        //public IPagedResult<TEntity> GetIndexPagedResult<TEntity>(int pageIndex, int pageSize, IQueryable<TEntity> query)
        //     where TEntity : class
        //{
        //    return this.CurrentSession.GetIndexPagedResult(pageIndex, pageSize, query);
        //}

    }
}
