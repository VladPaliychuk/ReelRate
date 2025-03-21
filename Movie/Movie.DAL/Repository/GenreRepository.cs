using Movie.DAL.Data;
using Movie.DAL.Entities;
using Movie.DAL.Infrastructure;
using Movie.DAL.Repository.Interfaces;

namespace Movie.DAL.Repository;

public class GenreRepository : GenericRepository<Genre>, IGenreRepository
{
    public GenreRepository(MovieContext context) : base(context)
    {
    }
}