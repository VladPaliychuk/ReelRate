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
}