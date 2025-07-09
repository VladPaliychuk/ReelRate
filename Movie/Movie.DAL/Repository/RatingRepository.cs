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
}