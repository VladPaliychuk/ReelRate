using System.Text.Json.Serialization;

namespace Movie.DAL.Entities.RelationsEntities;

public class FilmGenre
{
    public Guid FilmId { get; set; }
    public Guid GenreId { get; set; }

    [JsonIgnore] public Film Film { get; set; } = null!;
    [JsonIgnore] public Genre Genre { get; set; } = null!;
}