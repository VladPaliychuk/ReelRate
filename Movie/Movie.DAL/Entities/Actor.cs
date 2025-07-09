using System.Text.Json.Serialization;
using Movie.DAL.Entities.RelationsEntities;

namespace Movie.DAL.Entities;

public class Actor
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? BirthDate { get; set; }
    public string? BirthPlace { get; set; }
    public string? Info { get; set; }
    public string? Image { get; set; }

    [JsonIgnore] public ICollection<FilmActor>? FilmActors { get; set; }
}