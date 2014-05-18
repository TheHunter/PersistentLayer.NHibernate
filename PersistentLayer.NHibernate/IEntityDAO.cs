using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntityDAO<TEntity, TKey>
        : INhQueryableDAO<TEntity, TKey>, IPersisterDAO<TEntity, TKey>, ISessionContext
        where TEntity : class
    {
        
    }
}
