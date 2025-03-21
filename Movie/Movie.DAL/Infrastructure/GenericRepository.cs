using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Movie.DAL.Data;

namespace Movie.DAL.Infrastructure;

public class GenericRepository<TEntity> : IGenreicRepository<TEntity> where TEntity : class
{
    protected readonly MovieContext _context;
    private readonly DbSet<TEntity> _table;

    protected GenericRepository(MovieContext context)
    {
        _context = context;
        _table = context.Set<TEntity>();
    }

    /// <summary>
    /// GetByIdAsync
    /// </summary>
    /// <param name="id"></param>
    /// <returns>TEntity</returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _table.FindAsync(id)
               ?? throw new EntityNotFoundException($"{typeof(TEntity).Name} with id {id} not found.");
    }

    /// <summary>
    /// AddAsync
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual async Task AddAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(TEntity)} entity must not be null");
        }
        await _table.AddAsync(entity);
    }

    /// <summary>
    /// UpdateAsync
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task UpdateAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(TEntity)} entity must not be null");
        }
        await Task.Run(() => _table.Update(entity));
    }

    /// <summary>
    /// DeleteByIdAsync
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task DeleteByIdAsync(Guid id)
    {
        var entity = await GetByIdAsync(id) 
                     ?? throw new EntityNotFoundException($"{typeof(TEntity).Name} with id {id} not found. " +
                                                          $"Can't delete.");
        await Task.Run(() => _table.Remove(entity));
    }

    /// <summary>
    /// DeleteAsync
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
        }
        await Task.Run(() => _table.Remove(entity));
    }
}