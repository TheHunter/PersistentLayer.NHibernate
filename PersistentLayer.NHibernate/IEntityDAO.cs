using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Interface IEntityDAO
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    public interface IEntityDAO<TEntity, TKey>
        : INhQueryableDAO<TEntity, TKey>, IPersisterDAO<TEntity, TKey>, ISessionContext
        where TEntity : class
    {
        
    }
}
