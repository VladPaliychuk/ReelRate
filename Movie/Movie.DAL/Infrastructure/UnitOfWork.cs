using Movie.DAL.Data;
using Movie.DAL.Infrastructure.Interfaces;
using Movie.DAL.Repository;
using Movie.DAL.Repository.Interfaces;

namespace Movie.DAL.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    protected readonly MovieContext context;
    
    public UnitOfWork(MovieContext context,
        IActorRepository actorRepository,
        IFilmRepository filmRepository,
        IDirectorRepository directorRepository,
        IGenreRepository genreRepository,
        
        IFilmActorRepository filmActorRepository,
        IFilmDirectorRepository filmDirectorRepository,
        IFilmGenreRepository filmGenreRepository
        )
    {
        this.context = context;
        ActorRepository = actorRepository;
        FilmRepository = filmRepository;
        DirectorRepository = directorRepository;
        GenreRepository = genreRepository;
        
        FilmActorRepository = filmActorRepository;
        FilmDirectorRepository = filmDirectorRepository;
        FilmGenreRepository = filmGenreRepository;
    }
    
    public IActorRepository ActorRepository { get; }
    public IDirectorRepository DirectorRepository { get; }
    public IFilmRepository FilmRepository { get; }
    public IGenreRepository GenreRepository { get; }
    
    public IFilmActorRepository FilmActorRepository { get; }
    public IFilmDirectorRepository FilmDirectorRepository { get; }
    public IFilmGenreRepository FilmGenreRepository { get; }
    
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}