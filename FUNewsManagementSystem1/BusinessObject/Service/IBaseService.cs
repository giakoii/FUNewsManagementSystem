using System.Linq.Expressions;

namespace BusinessObject.Service;

/// <summary>
/// Interface Base Service
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IBaseService<TEntity, TKey> where TEntity : class
{
    IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate = null!, bool isTracking = false,  params Expression<Func<TEntity, object>>[] includes);
    
    TEntity GetById(TKey id);
    
    Task<TEntity> GetByIdAsync(TKey id);
}
