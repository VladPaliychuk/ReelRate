using System.ComponentModel.DataAnnotations;

namespace Movie.BLL.DTOs.ActorDTOs;

public class ActorCreateDto
{
    [Required]
    [MaxLength(40)]
    public string FullName { get; set; } = null!;
    [MaxLength(10)]
    public string? BirthDate { get; set; }
    [MaxLength(100)]
    public string? BirthPlace { get; set; }
    [Required]
    [MaxLength(1000)]
    public string? Info { get; set; }
    [MaxLength(1000)]
    public string? ImageFile { get; set; }
}