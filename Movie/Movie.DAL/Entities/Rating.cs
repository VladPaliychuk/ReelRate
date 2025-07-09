using System.ComponentModel.DataAnnotations;

namespace Movie.DAL.Entities;

public class Rating
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid FilmId { get; set; }
    public Film Film { get; set; } = null!;
    
    public decimal Score { get; set; }
    public int Count { get; set; }
}