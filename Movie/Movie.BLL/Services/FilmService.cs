using AutoMapper;
using Microsoft.Extensions.Logging;
using Movie.BLL.DTOs.FilmDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Entities;
using Movie.DAL.Exceptions;
using Movie.DAL.Infrastructure.Interfaces;
using Movie.DAL.Repository.Interfaces;

namespace Movie.BLL.Services;

public class FilmService : IFilmService
{
    private readonly IFilmRepository _filmRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DirectorService> _logger;
    
    public FilmService(
        IFilmRepository filmRepository, 
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ILogger<DirectorService> logger)
    {
        _filmRepository = filmRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<IEnumerable<FilmGetDto>> GetFilms()
    {
        var films = await _filmRepository.GetAllWithRatingAsync();
        return _mapper.Map<IEnumerable<FilmGetDto>>(films);
    }
    
    public async Task<FilmGetDto> GetFilmByIdAsync(Guid id)
    {
        var film = await _filmRepository.GetByIdWithRatingAsync(id);
        
        if (film == null)
        {
            _logger.LogWarning("Film with ID {Id} not found.", id);
            throw new EntityNotFoundException($"Genre with ID {id} not found.");
        }
        return _mapper.Map<FilmGetDto>(film);
    }
    
    public async Task<FilmGetDto> CreateFilmAsync(FilmCreateDto createDto)
    {
        var film = _mapper.Map<Film>(createDto);
        
        var createdFilm = await _filmRepository.AddAsync(film);

        if (createdFilm == null)
        {
            _logger.LogError("Failed to create film with title {Title}.", createDto.Name);
            throw new EntityNotFoundException("Failed to create film.");
        }

        var rating = new Rating
        {
            FilmId = createdFilm.Id,
            Score = 0,
            Count = 0
        };
        await _unitOfWork.RatingRepository.AddAsync(rating);
        
        _logger.LogInformation("Film with ID {Id} created successfully.", createdFilm.Id);
        _logger.LogInformation("Rating for Film with ID {Id} initialized.", createdFilm.Id);
        
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<FilmGetDto>(createdFilm);
    }
    
    public async Task<FilmGetDto> UpdateFilmAsync(FilmUpdateDto updateDto)
    {
        var film = await _filmRepository.GetByIdAsync(updateDto.Id);
        
        if (film == null)
        {
            _logger.LogWarning("Film with ID {Id} not found for update.", updateDto.Id);
            throw new EntityNotFoundException($"Film with ID {updateDto.Id} not found.");
        }
        
        _mapper.Map(updateDto, film);
        await _filmRepository.UpdateAsync(film);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<FilmGetDto>(film);
    }
    
    public async Task<bool> DeleteFilmAsync(Guid id)
    {
        var film = await _filmRepository.GetByIdAsync(id);
        
        if (film == null)
        {
            _logger.LogWarning("Film with ID {Id} not found for deletion.", id);
            throw new EntityNotFoundException($"Film with ID {id} not found.");
        }
        
        await _filmRepository.DeleteAsync(film);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}