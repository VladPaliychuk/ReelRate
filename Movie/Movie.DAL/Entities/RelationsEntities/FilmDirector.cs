using System.Text.Json.Serialization;

namespace Movie.DAL.Entities.RelationsEntities;

public class FilmDirector
{
    public Guid FilmId { get; set; }
    public Guid DirectorId { get; set; }

    [JsonIgnore] public Film Film { get; set; } = null!;
    [JsonIgnore] public Director Director { get; set; } = null!;
}