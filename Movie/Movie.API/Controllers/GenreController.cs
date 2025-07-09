using Microsoft.AspNetCore.Mvc;
using Movie.BLL.DTOs.GenreDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Data;
using Movie.DAL.Repository.Interfaces;

namespace Movie.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class GenreController : ControllerBase
{
    private readonly MovieContext _context;
    private readonly IGenreService _genreService;
    private readonly ILogger<GenreController> _logger;
    
    public GenreController(
        MovieContext context,
        IGenreService genreService,
        ILogger<GenreController> logger
        )
    {
        _context = context;
        _genreService = genreService;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GenreGetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGenres()
    {
        _logger.LogInformation("Attempting to retrieve all genres.");
        try
        {
            var genres = await _genreService.GetGenres();
            _logger.LogInformation("Successfully retrieved all genres.");
            return Ok(genres);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all genres.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GenreGetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGenreById(Guid id)
    {
        _logger.LogInformation($"Attempting to retrieve genre with ID: {id}");
        try
        {
            var genre = await _genreService.GetGenreByIdAsync(id);
            
            _logger.LogInformation($"Successfully retrieved genre with ID: {id}");
            return Ok(genre);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving genre with ID: {id}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(GenreGetDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateGenre([FromBody] GenreCreateDto genreCreateDto)
    {
        _logger.LogInformation("Attempting to create a new genre.");
        try
        {
            var createdGenre = await _genreService.CreateGenreAsync(genreCreateDto);
            _logger.LogInformation($"Successfully created genre with ID: {createdGenre.Id}");
            return CreatedAtAction(nameof(GetGenreById), new { id = createdGenre.Id }, createdGenre);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new genre.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(GenreGetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGenre([FromBody] GenreUpdateDto genreUpdateDto)
    {
        _logger.LogInformation($"Attempting to update genre with ID: {genreUpdateDto.Id}");
        try
        {
            var updatedGenre = await _genreService.UpdateGenreAsync(genreUpdateDto);
            _logger.LogInformation($"Successfully updated genre with ID: {updatedGenre.Id}");
            return Ok(updatedGenre);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating genre with ID: {genreUpdateDto.Id}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        _logger.LogInformation($"Attempting to delete genre with ID: {id}");
        try
        {
            await _genreService.DeleteGenreAsync(id);
            _logger.LogInformation($"Successfully deleted genre with ID: {id}");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting genre with ID: {id}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}