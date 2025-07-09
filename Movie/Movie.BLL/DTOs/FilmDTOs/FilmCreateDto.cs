using System.ComponentModel.DataAnnotations;

namespace Movie.BLL.DTOs.FilmDTOs;

public class FilmCreateDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(1000, ErrorMessage = "Fullname cannot exceed 1000 characters.")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    public string Description { get; set; } = null!;
    
    [Required(ErrorMessage = "ReleaseDate is required.")]
    [StringLength(10, ErrorMessage = "ReleaseDate cannot exceed 10 characters.")]
    public string ReleaseDate { get; set; } = null!;
    
    [Required(ErrorMessage = "Duration is required.")]
    [StringLength(15, ErrorMessage = "Duration cannot exceed 15 characters.")]
    public string Duration { get; set; } = null!;
    
    [Required(ErrorMessage = "Country is required.")]
    [StringLength(20, ErrorMessage = "Country cannot exceed 20 characters.")]
    public string Country { get; set; } = null!;
    
    [Required(ErrorMessage = "AgeRestriction is required.")]
    [StringLength(3, ErrorMessage = "AgeRestriction cannot exceed 3 characters.")]
    public string AgeRestriction { get; set; } = null!;
    
    [StringLength(1000, ErrorMessage = "Image file path cannot exceed 1000 characters.")]
    public string? Image { get; set; }
    
    [StringLength(3, ErrorMessage = "Rating cannot exceed 3 characters.")]
    public string? Rating { get; set; }
}