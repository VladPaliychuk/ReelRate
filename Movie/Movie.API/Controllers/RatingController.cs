using Microsoft.AspNetCore.Mvc;
using Movie.BLL.DTOs.RatingDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Data;

namespace Movie.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RatingController : ControllerBase
{
    private readonly ILogger<RatingController> _logger;
    private readonly IRatingService _ratingService;
    private readonly MovieContext _context;
    
    public RatingController(
        ILogger<RatingController> logger,
        IRatingService ratingService,
        MovieContext context)
    {
        _logger = logger;
        _ratingService = ratingService;
        _context = context;
    }

    [HttpPut]
    [Route("update")]
    [ProducesResponseType(typeof(RatingGetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRating(RatingUpdateDto updateDto)
    {
        _logger.LogInformation($"Attempting to update rating with Film ID: {updateDto.FilmId}");
        try
        {
            var updatedRating = await _ratingService.UpdateRatingAsync(updateDto);
            _logger.LogInformation($"Successfully updated rating with ID: {updatedRating.Id}");
            return Ok(updatedRating);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the rating.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}