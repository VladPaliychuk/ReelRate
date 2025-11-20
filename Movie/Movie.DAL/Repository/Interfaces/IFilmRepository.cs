using Movie.DAL.Entities;
using Movie.DAL.Infrastructure.Interfaces;

namespace Movie.DAL.Repository.Interfaces;

public interface IFilmRepository : IGenericRepository<Film>
{
    Task<IEnumerable<Film>> GetAllWithRatingAsync();
    Task<Film?> GetByIdWithRatingAsync(Guid id);
    Task<Film?> GetByIdWithRelationsAsync(Guid id);
}