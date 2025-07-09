using Movie.BLL.DTOs.RatingDTOs;

namespace Movie.BLL.Services.Interfaces;

public interface IRatingService
{
    Task<RatingGetDto> UpdateRatingAsync(RatingUpdateDto createDto);
}