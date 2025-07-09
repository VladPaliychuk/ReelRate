namespace Movie.BLL.DTOs.RatingDTOs;

public class RatingGetDto
{
    public Guid Id { get; set; }
    
    public Guid FilmId { get; set; }
    
    public decimal Score { get; set; }
    public int Count { get; set; }
}