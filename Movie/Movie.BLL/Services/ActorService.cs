using AutoMapper;
using Microsoft.Extensions.Logging;
using Movie.BLL.DTOs.ActorDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Entities;
using Movie.DAL.Exceptions;
using Movie.DAL.Infrastructure.Interfaces;
using Movie.DAL.Repository.Interfaces;

namespace Movie.BLL.Services;

public class ActorService : IActorService
{
    private readonly IActorRepository _actorRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ActorService> _logger;
    
    public ActorService(
        IActorRepository actorRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<ActorService> logger
        )
    {
        _actorRepository = actorRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<IEnumerable<ActorGetDto>> GetActors()
    {
        var actors = await _actorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ActorGetDto>>(actors);
    }
    
    public async Task<ActorGetDto?> GetActorByIdAsync(Guid id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);

        if (actor == null)
        {
            _logger.LogWarning("Actor with ID {Id} not found.", id);
            throw new EntityNotFoundException($"Actor with ID {id} not found.");
        }
        
        return _mapper.Map<ActorGetDto>(actor);
    }
    
    public async Task<ActorGetDto> CreateActorAsync(ActorCreateDto createDto)
    {
        var actor = _mapper.Map<Actor>(createDto);
        
        var createdActor = await _actorRepository.AddAsync(actor);
        
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ActorGetDto>(createdActor);
    }
    
    public async Task<ActorGetDto?> UpdateActorAsync(ActorUpdateDto updateDto)
    {
        var actor = await _actorRepository.GetByIdAsync(updateDto.Id);
        
        if (actor == null)
        {
            _logger.LogWarning("Actor with ID {Id} not found.", updateDto.Id);
            throw new EntityNotFoundException($"Actor with ID {updateDto.Id} not found.");
        }

        _mapper.Map(updateDto, actor);
        await _actorRepository.UpdateAsync(actor);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ActorGetDto>(actor);
    }
    
    public async Task<bool> DeleteActorAsync(Guid id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);

        if (actor == null)
        {
            _logger.LogWarning("Actor with ID {Id} not found.", id);
            throw new EntityNotFoundException($"Actor with ID {id} not found.");
        }
        
        await _actorRepository.DeleteAsync(actor);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
