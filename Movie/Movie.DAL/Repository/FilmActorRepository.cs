using Movie.DAL.Data;
using Movie.DAL.Entities.RelationsEntities;
using Movie.DAL.Infrastructure;
using Movie.DAL.Repository.Interfaces;

namespace Movie.DAL.Repository;

public class FilmActorRepository : GenericRepository<FilmActor>, IFilmActorRepository
{
    public FilmActorRepository(MovieContext context) : base(context)
    {
    }
}