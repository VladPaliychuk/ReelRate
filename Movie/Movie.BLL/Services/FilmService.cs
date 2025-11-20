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
            throw new EntityNotFoundException($"Film with ID {id} not found.");
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
        var actorNames = dto.Actors?.Select(a => a.FullName).ToList() ?? new List<string>();
        var existingActors = await _unitOfWork.ActorRepository.FindAsync(a =>
            actorNames.Contains(a.FullName, StringComparer.OrdinalIgnoreCase));
        var filmActorsToAdd = new List<FilmActor>();

        foreach(var actorDto in dto.Actors ?? Enumerable.Empty<ActorCreateDto>())
        {
            var actorEntity = existingActors.FirstOrDefault(a =>
                string.Equals(a.FullName, actorDto.FullName, StringComparison.OrdinalIgnoreCase));

            if (actorEntity == null)
            {
                actorEntity = _mapper.Map<Actor>(actorDto);
                actorEntity = await _unitOfWork.ActorRepository.AddAsync(actorEntity);
            }

            filmActorsToAdd.Add(new FilmActor
            {
                FilmId = createdFilm.Id,
                ActorId = actorEntity.Id
            });
        }

        await _unitOfWork.FilmActorRepository.AddRangeAsync(filmActorsToAdd);

        // Genres
        var genreNames = dto.Genres?.Select(g => g.Name).ToList() ?? new List<string>();
        var existingGenres = await _unitOfWork.GenreRepository.FindAsync(g =>
            genreNames.Contains(g.Name, StringComparer.OrdinalIgnoreCase));
        var filmGenresToAdd = new List<FilmGenre>();

        foreach(var genre in dto.Genres ?? Enumerable.Empty<GenreCreateDto>())
        {
            var genreEntity = existingGenres.FirstOrDefault(g =>
                string.Equals(g.Name, genre.Name, StringComparison.OrdinalIgnoreCase));

            if (genreEntity == null)
            {
                genreEntity = _mapper.Map<Genre>(genre);
                genreEntity = await _unitOfWork.GenreRepository.AddAsync(genreEntity);
            }

            filmGenresToAdd.Add(new FilmGenre
            {
                FilmId = createdFilm.Id,
                GenreId = genreEntity.Id
            });
        }

        await _unitOfWork.FilmGenreRepository.AddRangeAsync(filmGenresToAdd);

        // Directors
        var directorNames = dto.Directors?.Select(d => d.FullName).ToList() ?? new List<string>();
        var existingDirectors = await _unitOfWork.DirectorRepository.FindAsync(d =>
            directorNames.Contains(d.FullName, StringComparer.OrdinalIgnoreCase));
        var filmDirectorsToAdd = new List<FilmDirector>();

        foreach(var director in dto.Directors ?? Enumerable.Empty<DirectorCreateDto>())
        {
            var directorEntity = existingDirectors.FirstOrDefault(d =>
                string.Equals(d.FullName, director.FullName, StringComparison.OrdinalIgnoreCase));

            if (directorEntity == null)
            {
                directorEntity = _mapper.Map<Director>(director);
                directorEntity = await _unitOfWork.DirectorRepository.AddAsync(directorEntity);
            }

            filmDirectorsToAdd.Add(new FilmDirector
            {
                FilmId = createdFilm.Id,
                DirectorId = directorEntity.Id
            });
        }

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Film with ID {Id} and its relations created successfully.", createdFilm.Id);

        return dto;
    }

    public async Task<FilmWithRelationsGetDto> GetFilmWithRelationsAsync(Guid id)
    {
        var film = await _filmRepository.GetByIdWithRelationsAsync(id);
        if (film == null)
        {
            _logger.LogWarning("Film with ID {Id} not found.", id);
            throw new EntityNotFoundException($"Film with ID {id} not found.");
        }

        return new FilmWithRelationsGetDto
        {
            Film = _mapper.Map<FilmGetDto>(film),

            Actors = _mapper.Map<List<ActorGetDto>>(
            film.FilmActors.Select(fa => fa.Actor)
        ),

            Genres = _mapper.Map<List<GenreGetDto>>(
            film.FilmGenres.Select(fg => fg.Genre)
        ),

            Directors = _mapper.Map<List<DirectorGetDto>>(
            film.FilmDirectors.Select(fd => fd.Director)
        )
        };

        /*var filmActors = await _unitOfWork.FilmActorRepository.FindAsync(fa => fa.FilmId == id);
        var actors = new List<ActorGetDto>();
        foreach (var fa in filmActors ?? Enumerable.Empty<FilmActor>())
        {
            var actor = await _unitOfWork.ActorRepository.GetByIdAsync(fa.ActorId);
            actors.Add(_mapper.Map<ActorGetDto>(actor));
        }

        var filmGenres = await _unitOfWork.FilmGenreRepository.FindAsync(fg => fg.FilmId == id);
        var genres = new List<GenreGetDto>();
        foreach (var fg in filmGenres ?? Enumerable.Empty<FilmGenre>())
        {
            var genre = await _unitOfWork.GenreRepository.GetByIdAsync(fg.GenreId);
            genres.Add(_mapper.Map<GenreGetDto>(genre));
        }

        var filmDirectors = await _unitOfWork.FilmDirectorRepository.FindAsync(fd => fd.FilmId == id);
        var directors = new List<DirectorGetDto>();
        foreach (var fd in filmDirectors ?? Enumerable.Empty<FilmDirector>())
        {
            var director = await _unitOfWork.DirectorRepository.GetByIdAsync(fd.DirectorId);
            directors.Add(_mapper.Map<DirectorGetDto>(director));
        }

        return new FilmWithRelationsGetDto
        {
            Film = filmDto,
            Actors = actors,
            Genres = genres,
            Directors = directors
        };*/
    }

    public async Task<FilmWithRelationsCreateDto> UpdateFilmWithRelationsAsync(Guid filmId, FilmWithRelationsCreateDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        var film = await _filmRepository.GetByIdWithRelationsAsync(filmId);
        if (film == null)
        {
            _logger.LogWarning("Film with ID {Id} not found for update.", filmId);
            throw new EntityNotFoundException($"Film with ID {filmId} not found.");
        }

        // update film fields
        _mapper.Map(dto.Film, film);
        await _filmRepository.UpdateAsync(film);

        var filmActors = await _unitOfWork.FilmActorRepository.FindAsync(fa => fa.FilmId == filmId);
        if (filmActors != null && filmActors.Any())
        {
            await _unitOfWork.FilmActorRepository.DeleteRangeAsync(filmActors);
        }

        var filmGenres = await _unitOfWork.FilmGenreRepository.FindAsync(fg => fg.FilmId == filmId);
        if (filmGenres != null && filmGenres.Any())
        {
            await _unitOfWork.FilmGenreRepository.DeleteRangeAsync(filmGenres);
        }

        var filmDirectors = await _unitOfWork.FilmDirectorRepository.FindAsync(fd => fd.FilmId == filmId);
        if (filmDirectors != null && filmDirectors.Any())
        {
            await _unitOfWork.FilmDirectorRepository.DeleteRangeAsync(filmDirectors);
        }

        await _unitOfWork.SaveChangesAsync();

        // re-add relations 
        // Actors
        var actorNames = dto.Actors?.Select(a => a.FullName).ToList() ?? new List<string>();
        var existingActors = await _unitOfWork.ActorRepository.FindAsync(a =>
            actorNames.Contains(a.FullName, StringComparer.OrdinalIgnoreCase));
        var filmActorsToAdd = new List<FilmActor>();

        // deduplication
        var uniqueActors = dto.Actors?
            .GroupBy(a => a.FullName.Trim().ToLower(), StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToList() ?? new List<ActorCreateDto>();

        foreach(var actor in uniqueActors)
        {
            _logger.LogInformation($"{actor.FullName}");
        }

        foreach (var actorDto in uniqueActors ?? Enumerable.Empty<ActorCreateDto>())
        {
            var actorEntity = existingActors.FirstOrDefault(a =>
                string.Equals(a.FullName, actorDto.FullName, StringComparison.OrdinalIgnoreCase));

            if (actorEntity == null)
            {
                actorEntity = _mapper.Map<Actor>(actorDto);
                actorEntity = await _unitOfWork.ActorRepository.AddAsync(actorEntity);
            }

            filmActorsToAdd.Add(new FilmActor
            {
                FilmId = film.Id,
                ActorId = actorEntity.Id
            });
        }

        await _unitOfWork.FilmActorRepository.AddRangeAsync(filmActorsToAdd);

        // Genres
        var genreNames = dto.Genres?.Select(g => g.Name).ToList() ?? new List<string>();
        var existingGenres = await _unitOfWork.GenreRepository.FindAsync(g =>
            genreNames.Contains(g.Name, StringComparer.OrdinalIgnoreCase));
        var filmGenresToAdd = new List<FilmGenre>();

        // deduplication
        var uniqueGenres = dto.Genres?
            .GroupBy(a => a.Name, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToList() ?? new List<GenreCreateDto>();

        foreach (var genre in uniqueGenres ?? Enumerable.Empty<GenreCreateDto>())
        {
            var genreEntity = existingGenres.FirstOrDefault(g =>
                string.Equals(g.Name, genre.Name, StringComparison.OrdinalIgnoreCase));

            if (genreEntity == null)
            {
                genreEntity = _mapper.Map<Genre>(genre);
                genreEntity = await _unitOfWork.GenreRepository.AddAsync(genreEntity);
            }

            filmGenresToAdd.Add(new FilmGenre
            {
                FilmId = film.Id,
                GenreId = genreEntity.Id
            });
        }

        await _unitOfWork.FilmGenreRepository.AddRangeAsync(filmGenresToAdd);

        // Directors
        var directorNames = dto.Directors?.Select(d => d.FullName).ToList() ?? new List<string>();
        var existingDirectors = await _unitOfWork.DirectorRepository.FindAsync(d =>
            directorNames.Contains(d.FullName, StringComparer.OrdinalIgnoreCase));
        var filmDirectorsToAdd = new List<FilmDirector>();

        // deduplication
        var uniqueDirectors = dto.Directors?
            .GroupBy(a => a.FullName, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToList() ?? new List<DirectorCreateDto>();

        foreach (var director in uniqueDirectors ?? Enumerable.Empty<DirectorCreateDto>())
        {
            var directorEntity = existingDirectors.FirstOrDefault(d =>
                string.Equals(d.FullName, director.FullName, StringComparison.OrdinalIgnoreCase));

            if (directorEntity == null)
            {
                directorEntity = _mapper.Map<Director>(director);
                directorEntity = await _unitOfWork.DirectorRepository.AddAsync(directorEntity);
            }

            filmDirectorsToAdd.Add(new FilmDirector
            {
                FilmId = film.Id,
                DirectorId = directorEntity.Id
            });
        }

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Film with ID {Id} and its relations updated successfully.", filmId);

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