using AutoMapper;
using Microsoft.Extensions.Logging;
using Movie.BLL.DTOs.GenreDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Entities;
using Movie.DAL.Exceptions;
using Movie.DAL.Infrastructure.Interfaces;
using Movie.DAL.Repository.Interfaces;

namespace Movie.BLL.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GenreService> _logger;
    
    public GenreService(
        IGenreRepository genreRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GenreService> logger
    )
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<IEnumerable<GenreGetDto>> GetGenres()
    {
        var genres = await _genreRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GenreGetDto>>(genres);
    }
    
    public async Task<GenreGetDto> GetGenreByIdAsync(Guid id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        
        if (genre == null)
        {
            _logger.LogWarning("Genre with ID {Id} not found.", id);
            throw new EntityNotFoundException($"Genre with ID {id} not found.");
        }
        
        return _mapper.Map<GenreGetDto>(genre);
    }
    
    public async Task<GenreGetDto> CreateGenreAsync(GenreCreateDto createDto)
    {
        var genre = _mapper.Map<Genre>(createDto);
        var createdGenre = await _genreRepository.AddAsync(genre);
        
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<GenreGetDto>(createdGenre);
    }
    
    public async Task<GenreGetDto> UpdateGenreAsync(GenreUpdateDto updateDto)
    {
        var genre = await _genreRepository.GetByIdAsync(updateDto.Id);
        
        if (genre == null)
        {
            _logger.LogWarning("Genre with ID {Id} not found for update.", updateDto.Id);
            throw new EntityNotFoundException($"Genre with ID {updateDto.Id} not found.");
        }
        
        _mapper.Map(updateDto, genre);
        await _genreRepository.UpdateAsync(genre);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<GenreGetDto>(genre);
    }
    
    public async Task<bool> DeleteGenreAsync(Guid id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        
        if (genre == null)
        {
            _logger.LogWarning("Genre with ID {Id} not found for deletion.", id);
            throw new EntityNotFoundException($"Genre with ID {id} not found.");
        }
        
        await _genreRepository.DeleteAsync(genre);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}