using Microsoft.EntityFrameworkCore;
using Movie.DAL.Data;
using Movie.DAL.Entities;
using Movie.DAL.Infrastructure;
using Movie.DAL.Repository.Interfaces;

namespace Movie.DAL.Repository;

public class RatingRepository : GenericRepository<Rating>, IRatingRepository
{
    public RatingRepository(MovieContext context) : base(context)
    {
        
    }
    
    public async Task<Rating> GetByFilmIdAsync(Guid filmId)
    {
        return await _context.Ratings
            .FirstOrDefaultAsync(r => r.FilmId == filmId)
            ?? throw new KeyNotFoundException($"Rating for film with ID {filmId} not found.");
    }
}