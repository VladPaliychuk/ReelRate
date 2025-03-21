namespace Movie.BLL.DTOs.ActorDTOs;

public class ActorGetDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? BirthDate { get; set; }
    public string? BirthPlace { get; set; }
    public string? Description { get; set; }
    public string? ImageFile { get; set; }
}