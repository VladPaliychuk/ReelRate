using System.Text.Json.Serialization;

namespace Movie.DAL.Entities.RelationsEntity;

public class FilmActor
{
    public Guid FilmId { get; set; }
    public Guid ActorId { get; set; }

    [JsonIgnore] public Film Film { get; set; } = null!;
    [JsonIgnore] public Actor Actor { get; set; } = null!;
}