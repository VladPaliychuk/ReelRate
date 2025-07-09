using System.Text.Json.Serialization;
using Movie.DAL.Entities.RelationsEntities;

namespace Movie.DAL.Entities;

public class Genre
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    [JsonIgnore] public ICollection<FilmGenre>? FilmGenres { get; set; }
}