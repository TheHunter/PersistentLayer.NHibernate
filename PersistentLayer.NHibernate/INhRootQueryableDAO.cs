using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public interface INhRootQueryableDAO<in TRootEntity, TEntity>
        : IRootQueryableDAO<TRootEntity, TEntity>
        where TEntity : class, TRootEntity
        where TRootEntity : class
    {
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
    }

    /// <summary>
    /// 
    /// </summary>
    public interface INhRootQueryableDAO<in TRootEntity>
        : IRootQueryableDAO<TRootEntity>
        where TRootEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable<TEntity>() where TEntity : TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable<TEntity>(CacheMode mode) where TEntity : TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable<TEntity>(string region) where TEntity : TRootEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        IQueryable<TEntity> ToIQueryable<TEntity>(CacheMode mode, string region) where TEntity : TRootEntity;
    }

}
