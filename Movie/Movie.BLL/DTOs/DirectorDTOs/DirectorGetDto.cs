namespace Movie.BLL.DTOs.DirectorDTOs;

public class DirectorGetDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? BirthDate { get; set; }
    public string? BirthPlace { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
}