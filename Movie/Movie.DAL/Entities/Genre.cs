using System.Text.Json.Serialization;
using Movie.DAL.Entities.RelationsEntity;

namespace Movie.DAL.Entities;

public class Genre
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Image { get; set; }
    
    [JsonIgnore] public ICollection<FilmGenre>? FilmGenres { get; set; }
}