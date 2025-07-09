using System.ComponentModel.DataAnnotations;

namespace Movie.BLL.DTOs.RatingDTOs;

public class RatingUpdateDto
{
    [Required(ErrorMessage = "FilmId is required.")]
    public Guid FilmId { get; set; }
    
    [Required(ErrorMessage = "Score is required.")]
    [Range(0, 10, ErrorMessage = "Score must be between 0 and 10.")]
    public decimal Score { get; set; }
}