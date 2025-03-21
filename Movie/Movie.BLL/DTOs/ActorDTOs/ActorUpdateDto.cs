namespace Movie.BLL.DTOs.ActorDTOs;

public class ActorUpdateDto
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? BirthDate { get; set; }
    public string? BirthPlace { get; set; }
    public string? Description { get; set; }
    public string? ImageFile { get; set; }
}