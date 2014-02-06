using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public interface INhPersisterDAO
        : IPersisterDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        INhTransactionProvider GetNhTransactionProvider();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface INhPersisterDAO<TEntity, TKey>
        : IPersisterDAO<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        INhTransactionProvider GetNhTransactionProvider();
    }
}
