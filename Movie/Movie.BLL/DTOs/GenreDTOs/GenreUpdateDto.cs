using System.ComponentModel.DataAnnotations;

namespace Movie.BLL.DTOs.GenreDTOs;

public class GenreUpdateDto
{
    [Required(ErrorMessage = "Id is required.")]
    [StringLength(maximumLength: 15, MinimumLength = 15, ErrorMessage = "Id must be exactly 15 characters long.")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(15, ErrorMessage = "Name cannot exceed 15 characters.")]
    public string Name { get; set; } = null!;
}