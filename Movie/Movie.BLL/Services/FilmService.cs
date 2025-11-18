using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movie.BLL.DTOs.ActorDTOs;
using Movie.BLL.DTOs.DirectorDTOs;
using Movie.BLL.DTOs.FilmDTOs;
using Movie.BLL.DTOs.GenreDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Entities;
using Movie.DAL.Entities.RelationsEntities;
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

    //TODO:  adding explicit repository methods like GetByFullNameAsync or GetByNameAsync to avoid client-side enumeration.
    public async Task<FilmWithRelationsCreateDto> CreateFilmWithRelations(FilmWithRelationsCreateDto dto)
    {
        // Film
        var filmEntity = _mapper.Map<Film>(dto.Film);
        var createdFilm = await _filmRepository.AddAsync(filmEntity);

        if (createdFilm == null)
        {
            _logger.LogError("Failed to create film with relations.");
            throw new EntityNotFoundException("Failed to create film.");
        }

        // Rating
        var rating = new Rating
        {
            FilmId = createdFilm.Id,
            Score = 0,
            Count = 0
        };
        await _unitOfWork.RatingRepository.AddAsync(rating);

        // Actors
        foreach (var actorDto in dto.Actors ?? Enumerable.Empty<ActorCreateDto>())
        {
            var existingActors = await _unitOfWork.ActorRepository.FindAsync(a =>
                string.Equals(a.FullName, actorDto.FullName, StringComparison.OrdinalIgnoreCase));

            Actor actorEntity;

            if (existingActors != null && existingActors.Any())
            {
                actorEntity = existingActors.First();
            }
            else
            {
                actorEntity = _mapper.Map<Actor>(actorDto);
                actorEntity = await _unitOfWork.ActorRepository.AddAsync(actorEntity);
            }

            var filmActor = new FilmActor
            {
                FilmId = createdFilm.Id,
                ActorId = actorEntity.Id
            };
            await _unitOfWork.FilmActorRepository.AddAsync(filmActor);
        }

        // Genres
        foreach (var genreDto in dto.Genres ?? Enumerable.Empty<GenreCreateDto>())
        {
            var existingGenres = await _unitOfWork.GenreRepository.FindAsync(g =>
                string.Equals(g.Name, genreDto.Name, StringComparison.OrdinalIgnoreCase));

            Genre genreEntity;

            if (existingGenres != null && existingGenres.Any())
            {
                genreEntity = existingGenres.First();
            }
            else
            {
                genreEntity = _mapper.Map<Genre>(genreDto);
                genreEntity = await _unitOfWork.GenreRepository.AddAsync(genreEntity);
            }

            var filmGenre = new FilmGenre
            {
                FilmId = createdFilm.Id,
                GenreId = genreEntity.Id
            };
            await _unitOfWork.FilmGenreRepository.AddAsync(filmGenre);
        }

        // Directors
        foreach (var directorDto in dto.Directors ?? Enumerable.Empty<DirectorCreateDto>())
        {
            var existingDirectors = await _unitOfWork.DirectorRepository.FindAsync(d =>
                string.Equals(d.FullName, directorDto.FullName, StringComparison.OrdinalIgnoreCase));

            Director directorEntity;

            if (existingDirectors != null && existingDirectors.Any())
            {
                directorEntity = existingDirectors.First();
            }
            else
            {
                directorEntity = _mapper.Map<Director>(directorDto);
                directorEntity = await _unitOfWork.DirectorRepository.AddAsync(directorEntity);
            }

            var filmDirector = new FilmDirector
            {
                FilmId = createdFilm.Id,
                DirectorId = directorEntity.Id
            };
            await _unitOfWork.FilmDirectorRepository.AddAsync(filmDirector);
        }
     
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Film with ID {Id} and its relations created successfully.", createdFilm.Id);

        return dto;
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