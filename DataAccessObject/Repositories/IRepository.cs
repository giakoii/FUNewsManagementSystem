using System.Linq.Expressions;

namespace DataAccessObject.Repositories;

public interface IRepository<T, U> where T : class
{
    IQueryable<T> GetBy(Expression<Func<T, bool>> predicate = null!, bool isTracking = false,  params Expression<Func<T, object>>[] includes);

    public Task<IQueryable<T>> GetByAsync(Expression<Func<T, 
                                        bool>> predicate = null!,
                                        bool isTracking = false,
                                        params Expression<Func<T, object>>[] includes);

    public T GetById(U id);
    
    public Task<TResult?> GetByIdAsync<TResult>(U id, Expression<Func<T, TResult>> members);

    public Task<T> GetByIdAsync(U id);

    public bool Add(T entity);

    public void Update(T entity);
    
    public bool Delete(T entity);
    
    public bool Delete(U id);
}