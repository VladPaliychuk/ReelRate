using Microsoft.EntityFrameworkCore;
using Movie.DAL.Data;
using Movie.DAL.Entities;
using Movie.DAL.Infrastructure;
using Movie.DAL.Repository.Interfaces;

namespace Movie.DAL.Repository;

public class FilmRepository : GenericRepository<Film>, IFilmRepository
{
    public FilmRepository(MovieContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Film>> GetAllWithRatingAsync()
    {
        IQueryable<Film> query = _context.Films;
        query = query.Include(f => f.Rating!.Score);
        
        return await query.ToListAsync();
    }

    public Task<Film?> GetByIdWithRatingAsync(Guid id)
    {
        IQueryable<Film> query = _context.Films;
        query = query.Include(f => f.Rating!.Score);
        
        return query.FirstOrDefaultAsync(f => f.Id == id);
    }
}