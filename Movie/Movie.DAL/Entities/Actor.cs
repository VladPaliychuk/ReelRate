using System.Text.Json.Serialization;
using Movie.DAL.Entities.RelationsEntity;

namespace Movie.DAL.Entities;

public class Actor
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Info { get; set; } = null!;
    public string? Image { get; set; }

    [JsonIgnore] public ICollection<FilmActor>? FilmActors { get; set; }
}