using Microsoft.AspNetCore.Mvc;
using Movie.BLL.DTOs.ActorDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Data;

namespace Movie.API.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class ActorController : ControllerBase
{
    private readonly MovieContext _context;
    private readonly ILogger<ActorController> _logger;
    private readonly IActorService _actorService;
    
    public ActorController(
        MovieContext context,
        ILogger<ActorController> logger,
        IActorService actorService
        )
    {
        _context = context;
        _logger = logger;
        _actorService = actorService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ActorGetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActors()
    {
        _logger.LogInformation("Attempting to retrieve all actors.");
        try
        {
            var actors = await _actorService.GetActors();
            _logger.LogInformation("Successfully retrieved all actors.");
            return Ok(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all actors.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ActorGetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActorById(Guid id)
    {
        _logger.LogInformation($"Attempting to retrieve actor with ID: {id}");
        try
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
            {
                _logger.LogWarning($"Actor with ID: {id} not found.");
                return NotFound();
            }
            _logger.LogInformation($"Successfully retrieved actor with ID: {id}");
            return Ok(actor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving actor with ID: {id}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ActorGetDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateActor([FromBody] ActorCreateDto createDto)
    {
        _logger.LogInformation("Attempting to create a new actor.");
        try
        {
            var createdActor = await _actorService.CreateActorAsync(createDto);
            _logger.LogInformation($"Successfully created actor with ID: {createdActor.Id}");
            return CreatedAtAction(nameof(GetActorById), new { id = createdActor.Id }, createdActor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new actor.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateActor([FromBody] ActorUpdateDto updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        _logger.LogInformation($"Attempting to update actor with ID: {updateDto.Id}");
        
        try
        {
            var updatedActor = await _actorService.UpdateActorAsync(updateDto);
            if (updatedActor == null)
            {
                _logger.LogWarning($"Actor with ID: {updateDto.Id} not found for update.");
                return NotFound();
            }
            _logger.LogInformation($"Successfully updated actor with ID: {updateDto.Id}");
            return Ok(updatedActor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating actor with ID: {updateDto.Id}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteActor(Guid id)
    {
        _logger.LogInformation($"Attempting to delete actor with ID: {id}");
        try
        {
            var isDeleted = await _actorService.DeleteActorAsync(id);
            if (!isDeleted)
            {
                _logger.LogWarning($"Actor with ID: {id} not found for deletion.");
                return NotFound();
            }
            _logger.LogInformation($"Successfully deleted actor with ID: {id}");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting actor with ID: {id}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}