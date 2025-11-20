using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Movie.BLL.DTOs.FilmDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Data;
using Movie.DAL.Exceptions;

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

            _logger.LogInformation($"Successfully retrieved film with ID: {id}");
            return Ok(film);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving film by ID.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet("{id:guid}/with-relations")]
    [ProducesResponseType(typeof(FilmWithRelationsGetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFilmWithRelations([FromRoute] Guid id)
    {
        _logger.LogInformation($"Attempting to retrieve film with relations for ID: {id}");
        try
        {
            var result = await _filmService.GetFilmWithRelationsAsync(id);

            _logger.LogInformation($"Successfully retrieved film with relations for ID: {id}");
            return Ok(result);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning(ex, "Film not found.");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving film with relations.");
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

    [HttpPost("with-relations")]
    [ProducesResponseType(typeof(FilmWithRelationsCreateDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateFilmWithRelations([FromBody] FilmWithRelationsCreateDto dto)
    {
        _logger.LogInformation("Attempting to create a new film with relations.");
        try
        {
            var createdFilm = await _filmService.CreateFilmWithRelations(dto);

            _logger.LogInformation($"Successfully created film and its relations.");
            return Created(string.Empty, createdFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new film with relations.");
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

    [HttpPut("{filmId:guid}/with-relations")]
    [ProducesResponseType(typeof(FilmWithRelationsCreateDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateFilmWithRelationsAsync(Guid filmId, [FromBody] FilmWithRelationsCreateDto dto)
    {
        _logger.LogInformation($"Attempting to update relations of film with ID: {filmId}");

        try
        {
            var updatedFilm = await _filmService.UpdateFilmWithRelationsAsync(filmId, dto);
            _logger.LogInformation($"Successfully updated relations of film with ID: {filmId}");
            return Ok(updatedFilm);
        }
        catch (EntityNotFoundException)
        {
            _logger.LogWarning($"Film with ID: {filmId} not found for update.");
            return NotFound();
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

            _logger.LogInformation($"Successfully deleted film with ID: {id}");
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            return NotFound(ex.Message);
        }   
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the film.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}