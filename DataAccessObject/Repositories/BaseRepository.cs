using System.Linq.Expressions;
using DataAccessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly FUNewsManagementSystemContext _context;
    
    public BaseRepository(FUNewsManagementSystemContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Get all entities
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="isTracking"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null, bool isTracking = false, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (!isTracking)
        {
            query = query.AsNoTracking();
        }
        return query;
    }

    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    /// <summary>
    /// Add entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Add(T entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
        return true;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity"></param>
    public void Update(T entity)
    {
        _context.Update(entity);
        _context.SaveChanges();
    }

    /// <summary>
    /// Delete entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Delete(int id)
    {
        var entity = _context.Set<T>().Find(id);
        if (entity == null)
        {
            return false;
        }
        _context.Remove(entity);
        _context.SaveChanges();
        return true;
    }
}