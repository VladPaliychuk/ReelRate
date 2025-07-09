using System.ComponentModel.DataAnnotations;

namespace Movie.BLL.DTOs.GenreDTOs;

public class GenreCreateDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(15, ErrorMessage = "Name cannot exceed 15 characters.")]
    public string Name { get; set; } = null!;
}