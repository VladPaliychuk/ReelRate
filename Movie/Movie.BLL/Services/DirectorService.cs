using AutoMapper;
using Microsoft.Extensions.Logging;
using Movie.BLL.DTOs.DirectorDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Entities;
using Movie.DAL.Exceptions;
using Movie.DAL.Infrastructure.Interfaces;
using Movie.DAL.Repository.Interfaces;

namespace Movie.BLL.Services;

public class DirectorService : IDirectorService
{
    private readonly IDirectorRepository _directorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DirectorService> _logger;
    
    public DirectorService(
        IDirectorRepository directorRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<DirectorService> logger
    )
    {
        _directorRepository = directorRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<IEnumerable<DirectorGetDto>> GetDirectors()
    {
        var directors = await _directorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<DirectorGetDto>>(directors);
    }
    
    public async Task<DirectorGetDto> GetDirectorByIdAsync(Guid id)
    {
        var director = await _directorRepository.GetByIdAsync(id);
        
        if (director == null)
        {
            _logger.LogWarning("Director with ID {Id} not found.", id);
            throw new EntityNotFoundException($"Director with ID {id} not found.");
        }
        
        return _mapper.Map<DirectorGetDto>(director);
    }
    
    public async Task<DirectorGetDto> CreateDirectorAsync(DirectorCreateDto createDto)
    {
        var director = _mapper.Map<Director>(createDto);
        var createdDirector = await _directorRepository.AddAsync(director);
        
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<DirectorGetDto>(createdDirector);
    }
    
    public async Task<DirectorGetDto> UpdateDirectorAsync(DirectorUpdateDto updateDto)
    {
        var director = await _directorRepository.GetByIdAsync(updateDto.Id);
        
        if (director == null)
        {
            _logger.LogWarning("Director with ID {Id} not found for update.", updateDto.Id);
            throw new EntityNotFoundException($"Director with ID {updateDto.Id} not found.");
        }
        
        _mapper.Map(updateDto, director);
        await _directorRepository.UpdateAsync(director);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<DirectorGetDto>(director);
    }
    
    public async Task<bool> DeleteDirectorAsync(Guid id)
    {
        var director = await _directorRepository.GetByIdAsync(id);
        if (director == null)
        {
            _logger.LogWarning("Director with ID {Id} not found.", id);
            throw new EntityNotFoundException($"Director with ID {id} not found.");
        }

        await _directorRepository.DeleteAsync(director);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}