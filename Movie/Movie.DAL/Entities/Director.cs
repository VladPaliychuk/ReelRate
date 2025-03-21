using System.Text.Json.Serialization;
using Movie.DAL.Entities.RelationsEntity;

namespace Movie.DAL.Entities;

public class Director
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Info { get; set; } = null!;
    public string? Image { get; set; }

    [JsonIgnore] public ICollection<FilmDirector>? FilmDirectors { get; set; }
}