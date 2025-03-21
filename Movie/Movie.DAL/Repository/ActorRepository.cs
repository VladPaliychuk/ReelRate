using Movie.DAL.Data;
using Movie.DAL.Entities;
using Movie.DAL.Infrastructure;
using Movie.DAL.Repository.Interfaces;

namespace Movie.DAL.Repository;

public class ActorRepository : GenericRepository<Actor>, IActorRepository
{
    public ActorRepository(MovieContext context) : base(context)
    {
    }
}