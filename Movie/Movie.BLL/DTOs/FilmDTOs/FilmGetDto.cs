namespace Movie.BLL.DTOs.FilmDTOs;

public class FilmGetDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ReleaseDate { get; set; } = null!;
    public string Duration { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string AgeRestriction { get; set; } = null!;
    public string? Image { get; set; }
    public decimal? Rating { get; set; }
}