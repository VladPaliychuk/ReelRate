using Movie.DAL.Data;
using Movie.DAL.Entities.RelationsEntities;
using Movie.DAL.Infrastructure;
using Movie.DAL.Repository.Interfaces;

namespace Movie.DAL.Repository;

public class FilmGenreRepository : GenericRepository<FilmGenre>, IFilmGenreRepository
{
    public FilmGenreRepository(MovieContext context) : base(context)
    {
    }
}