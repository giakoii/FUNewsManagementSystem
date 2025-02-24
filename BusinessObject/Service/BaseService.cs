using System.Linq.Expressions;
using DataAccessObject.Repositories;

namespace BusinessObject.Service;

public class BaseService<TEntity, TKey> : IBaseService<TEntity, TKey>
    where TEntity : class
{
    protected readonly BaseRepository<TEntity, TKey> Repository;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="repository"></param>
    protected BaseService(BaseRepository<TEntity, TKey> repository)
    {
        Repository = repository;
    }

    /// <summary>
    /// Get all entities
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="isTracking"></param>
    /// <param name="includes"></param>
    /// <returns></returns>

    public IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate = null, bool isTracking = false, params Expression<Func<TEntity, object>>[] includes)
    {
        return Repository.GetBy(predicate, isTracking, includes);
    }

    /// <summary>
    /// Get entity by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual TEntity GetById(TKey id)
    {
        return Repository.GetById(id);
    }

    /// <summary>
    /// Get entity by Id async
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual Task<TEntity> GetByIdAsync(TKey id)
    {
        return Repository.GetByIdAsync(id);
    }
}