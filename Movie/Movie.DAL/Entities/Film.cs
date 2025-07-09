using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Movie.DAL.Entities.RelationsEntities;

namespace Movie.DAL.Entities;

public class Film
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ReleaseDate { get; set; } = null!;
    public string Duration { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string AgeRestriction { get; set; } = null!;
    public string? Image { get; set; }
    public string? Rating { get; set; }
    
    [JsonIgnore] public ICollection<FilmActor>? FilmActors { get; set; }
    [JsonIgnore] public ICollection<FilmGenre>? FilmGenres { get; set; }
    [JsonIgnore] public ICollection<FilmDirector>? FilmDirectors { get; set; }
}