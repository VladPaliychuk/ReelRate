using Microsoft.AspNetCore.Mvc;
using Movie.BLL.DTOs.FilmDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Data;

namespace Movie.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FilmController : ControllerBase
{
    private readonly MovieContext _context;
    private readonly ILogger<FilmController> _logger;
    private readonly IFilmService _filmService;
    
    public FilmController(
        MovieContext context,
        ILogger<FilmController> logger,
        IFilmService filmService
        )
    {
        _context = context;
        _logger = logger;
        _filmService = filmService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FilmGetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFilms()
    {
        _logger.LogInformation("Attempting to retrieve all films.");
        try
        {
            var films = await _filmService.GetFilms();
            _logger.LogInformation("Successfully retrieved all films.");
            return Ok(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all films.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FilmGetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFilmById(Guid id)
    {
        _logger.LogInformation($"Attempting to retrieve film with ID: {id}");
        try
        {
            var film = await _filmService.GetFilmByIdAsync(id);
            if (film == null)
            {
                _logger.LogWarning($"Film with ID: {id} not found.");
                return NotFound();
            }
            _logger.LogInformation($"Successfully retrieved film with ID: {id}");
            return Ok(film);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving film by ID.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(FilmGetDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateFilm([FromBody] FilmCreateDto createDto)
    {
        _logger.LogInformation("Attempting to create a new film.");
        try
        {
            var createdFilm = await _filmService.CreateFilmAsync(createDto);
            _logger.LogInformation($"Successfully created film with ID: {createdFilm.Id}");
            return CreatedAtAction(nameof(GetFilmById), new { id = createdFilm.Id }, createdFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new film.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(FilmGetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateFilm([FromBody] FilmUpdateDto updateDto)
    {
        _logger.LogInformation($"Attempting to update film with ID: {updateDto.Id}");
        try
        {
            var updatedFilm = await _filmService.UpdateFilmAsync(updateDto);
            _logger.LogInformation($"Successfully updated film with ID: {updatedFilm.Id}");
            return Ok(updatedFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the film.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFilm(Guid id)
    {
        _logger.LogInformation($"Attempting to delete film with ID: {id}");
        try
        {
            var isDeleted = await _filmService.DeleteFilmAsync(id);
            if (!isDeleted)
            {
                _logger.LogWarning($"Film with ID: {id} not found for deletion.");
                return NotFound();
            }
            _logger.LogInformation($"Successfully deleted film with ID: {id}");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the film.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}