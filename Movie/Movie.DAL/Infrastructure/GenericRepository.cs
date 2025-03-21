using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Movie.DAL.Data;
using Movie.DAL.Exceptions;
using Movie.DAL.Infrastructure.Interfaces;

namespace Movie.DAL.Infrastructure;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly MovieContext _context;
    private readonly DbSet<TEntity> _table;

    protected GenericRepository(MovieContext context)
    {
        _context = context;
        _table = context.Set<TEntity>();
    }
    
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _table.ToListAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _table.FindAsync(id)
               ?? throw new EntityNotFoundException($"{typeof(TEntity).Name} with id {id} not found.");
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(TEntity)} entity must not be null");
        }
        await _table.AddAsync(entity);
    }
    
    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentNullException(nameof(entities));
        
        await _table.AddRangeAsync(entities);
    }
    
    public virtual async Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate)
    {
        return await Task.Run(() => _table.Where(predicate).ToList());
    }
    
    public virtual async Task UpdateAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(TEntity)} entity must not be null");
        }
        await Task.Run(() => _table.Update(entity));
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentNullException(nameof(entities));
        
        _table.UpdateRange(entities);
        await Task.CompletedTask;
    }
    
    public virtual async Task DeleteByIdAsync(Guid id)
    {
        var entity = await GetByIdAsync(id) 
                     ?? throw new EntityNotFoundException($"{typeof(TEntity).Name} with id {id} not found. " +
                                                          $"Can't delete.");
        await Task.Run(() => _table.Remove(entity));
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
        }
        await Task.Run(() => _table.Remove(entity));
    }
    
    public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentNullException(nameof(entities));
        
        _table.RemoveRange(entities);
        /// <exception cref="ArgumentNullException"></exception>
        await Task.CompletedTask;
    }
}