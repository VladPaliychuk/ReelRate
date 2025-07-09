using System.ComponentModel.DataAnnotations;

namespace Movie.BLL.DTOs.FilmDTOs;

public class FilmUpdateDto
{
    [Required(ErrorMessage = "Id is required.")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(1000, ErrorMessage = "Fullname cannot exceed 1000 characters.")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = null!;
    
    [Required(ErrorMessage = "ReleaseDate is required.")]
    public string ReleaseDate { get; set; } = null!;
    
    [Required(ErrorMessage = "Duration is required.")]
    public string Duration { get; set; } = null!;
    
    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; } = null!;
    
    [Required(ErrorMessage = "AgeRestriction is required.")]
    public string AgeRestriction { get; set; } = null!;
    
    [StringLength(1000, ErrorMessage = "Image file path cannot exceed 1000 characters.")]
    public string? Image { get; set; }
}