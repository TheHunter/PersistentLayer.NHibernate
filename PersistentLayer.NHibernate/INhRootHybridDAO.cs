using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public interface INhRootHybridDAO<in TRootEntity, TEntity>
        : INhRootQueryableDAO<TRootEntity, TEntity>, IRootPersisterDAO<TRootEntity, TEntity>, ISessionContext
        where TRootEntity : class
        where TEntity : class, TRootEntity
    {
        
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRootEntity"></typeparam>
    public interface INhRootHybridDAO<in TRootEntity>
        : INhRootQueryableDAO<TRootEntity>, IRootPersisterDAO<TRootEntity>, ISessionContext
        where TRootEntity : class
    {
        
    }
}
