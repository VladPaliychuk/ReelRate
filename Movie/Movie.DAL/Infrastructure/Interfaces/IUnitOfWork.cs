using Movie.DAL.Repository.Interfaces;

namespace Movie.DAL.Infrastructure.Interfaces;

public interface IUnitOfWork
{
    public IActorRepository ActorRepository { get; }
    public IDirectorRepository DirectorRepository { get; }
    public IFilmRepository FilmRepository { get; }
    public IGenreRepository GenreRepository { get; }
    public IRatingRepository RatingRepository { get; }
    
    public IFilmActorRepository FilmActorRepository { get; }
    public IFilmDirectorRepository FilmDirectorRepository { get; }
    public IFilmGenreRepository FilmGenreRepository { get; }
    
    Task SaveChangesAsync();
}