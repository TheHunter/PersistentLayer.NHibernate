namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface ISessionDAO<TEntity, TKey>
        : IEntityDAO<TEntity, TKey>, ISessionContext
        where TEntity : class
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISessionDAO
        : IDomainDAO, ISessionContext
    {
    }
}
