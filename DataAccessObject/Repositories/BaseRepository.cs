using System.Linq.Expressions;
using DataAccessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject.Repositories;

/// <summary>
/// BaseRepository - Base class for all repositories
/// </summary>
/// <typeparam name="Entity"></typeparam>
/// <typeparam name="Type"></typeparam>
public class BaseRepository<Entity, Type> : IRepository<Entity, Type> where Entity : class
{
    protected readonly FUNewsManagementSystemContext _context;
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context"></param>
    public BaseRepository(FUNewsManagementSystemContext context)
    {
        _context = context;
    }
    
    private DbSet<Entity> DbSet => _context.Set<Entity>();

    public virtual IQueryable<Entity> Table => this.DbSet;
    
    /// <summary>
    /// Get all entities
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="isTracking"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    public IQueryable<Entity> GetBy(Expression<Func<Entity, bool>> predicate = null, bool isTracking = false, params Expression<Func<Entity, object>>[] includes)
    {
        IQueryable<Entity> query = _context.Set<Entity>();

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
    
    public Task<IQueryable<Entity>> GetByAsync(Expression<Func<Entity, bool>> predicate = null!,bool isTracking = false, params Expression<Func<Entity, object>>[] includes)
    {
        IQueryable<Entity> query = DbSet;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (includes != null)
        {
            query = includes.Aggregate(query, (current, inc) => current.Include(inc));
        }
        if (!isTracking)
        {
            query = query.AsNoTracking();
        }

        return Task.FromResult(query);
    }


    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Entity GetById(Type id)
    {
        return DbSet.Find(id);
    }
    
    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Entity> GetByIdAsync(Type id)
    {
        return await DbSet.FindAsync(id);
    }

    /// <summary>
    /// Add entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Add(Entity entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
        return true;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity"></param>
    public void Update(Entity entity)
    {
        _context.Update(entity);
        _context.SaveChanges();
    }
    
    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Delete(Entity entity)
    {
        _context.Remove(entity);
        return _context.SaveChanges() > 0;
    }
    
    public bool Delete(Type id)
    {
        var entity = GetById(id);
        if (entity == null)
        {
            return false;
        }
        return Delete(entity);
    }
}