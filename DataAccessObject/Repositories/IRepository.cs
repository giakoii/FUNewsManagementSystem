using System.Linq.Expressions;

namespace DataAccessObject.Repositories;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null!, bool isTracking = false,  params Expression<Func<T, object>>[] includes);
    
    T GetById(int id);

    bool Add(T entity);
    
    void Update(T entity);

    bool Delete(int id);
    bool Deletee(T entity);


}