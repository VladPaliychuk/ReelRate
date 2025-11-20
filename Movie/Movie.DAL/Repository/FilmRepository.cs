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
        query = query.Include(f => f.Rating);
        
        return await query.ToListAsync();
    }

    public Task<Film?> GetByIdWithRatingAsync(Guid id)
    {
        IQueryable<Film> query = _context.Films;
        query = query.Include(f => f.Rating);
        
        return query.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Film?> GetByIdWithRelationsAsync(Guid id)
    {
        return await _context.Films
            .Include(f => f.Rating)
            .Include(f => f.FilmActors).ThenInclude(fa => fa.Actor)
            .Include(f => f.FilmGenres).ThenInclude(fg => fg.Genre)
            .Include(f => f.FilmDirectors).ThenInclude(fd => fd.Director)
            .FirstOrDefaultAsync(f => f.Id == id);
    }
}