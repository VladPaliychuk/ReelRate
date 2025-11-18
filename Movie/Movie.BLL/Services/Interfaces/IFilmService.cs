using Movie.BLL.DTOs.FilmDTOs;

namespace Movie.BLL.Services.Interfaces;

public interface IFilmService
{
    Task<IEnumerable<FilmGetDto>> GetFilms();
    Task<FilmGetDto> GetFilmByIdAsync(Guid id);
    Task<FilmGetDto> CreateFilmAsync(FilmCreateDto createDto);
    Task<FilmWithRelationsCreateDto> CreateFilmWithRelations (FilmWithRelationsCreateDto dto);
    Task<FilmGetDto> UpdateFilmAsync(FilmUpdateDto updateDto);
    Task<bool> DeleteFilmAsync(Guid id);
}