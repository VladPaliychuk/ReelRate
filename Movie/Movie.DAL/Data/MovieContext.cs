using Microsoft.EntityFrameworkCore;
using Movie.DAL.Entities;
using Movie.DAL.Entities.RelationsEntity;

namespace Movie.DAL.Data;

public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<FilmDirector> FilmDirectors { get; set; }
    public DbSet<FilmActor> FilmActors { get; set; }
    public DbSet<FilmGenre> FilmGenres { get; set; }
    
    public DbSet<Film> Films { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}