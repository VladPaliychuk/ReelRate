using Movie.DAL.Data;
using Movie.DAL.Entities.RelationsEntity;
using Movie.DAL.Infrastructure;
using Movie.DAL.Repository.Interfaces;

namespace Movie.DAL.Repository;

public class FilmDirectorRepository : GenericRepository<FilmDirector>, IFilmDirectorRepository
{
    public FilmDirectorRepository(MovieContext context) : base(context)
    {
    }
}