using System.ComponentModel.DataAnnotations;

namespace Movie.BLL.DTOs.DirectorDTOs;

public class DirectorUpdateDto
{
    [Required(ErrorMessage = "Id is required.")]
    [StringLength(maximumLength: 15, MinimumLength = 15, ErrorMessage = "Id must be exactly 15 characters long.")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Fullname is required.")]
    [StringLength(40, ErrorMessage = "Fullname cannot exceed 40 characters.")]
    public string FullName { get; set; } = null!;
    
    [StringLength(10, ErrorMessage = "Birthdate cannot exceed 10 characters.")]
    public string? BirthDate { get; set; }
    
    [StringLength(100, ErrorMessage = "Birthplace cannot exceed 100 characters.")]
    public string? BirthPlace { get; set; }
    
    [StringLength(1000, ErrorMessage = "Info cannot exceed 1000 characters.")]
    public string? Info { get; set; }
    
    [StringLength(1000, ErrorMessage = "Image file path cannot exceed 1000 characters.")]
    public string? Image { get; set; }
}