using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Interface IDomainDAO
    /// </summary>
    public interface IDomainDAO
        : INhQueryableDAO, IPersisterDAO, ISessionContext
    {
        
    }
}
