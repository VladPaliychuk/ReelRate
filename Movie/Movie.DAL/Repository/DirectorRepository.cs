using Movie.DAL.Data;
using Movie.DAL.Entities;
using Movie.DAL.Infrastructure;
using Movie.DAL.Repository.Interfaces;

namespace Movie.DAL.Repository;

public class DirectorRepository : GenericRepository<Director>, IDirectorRepository
{
    public DirectorRepository(MovieContext context) : base(context)
    {
    }
}