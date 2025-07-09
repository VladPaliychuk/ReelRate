using Movie.BLL.DTOs.FilmDTOs;

namespace Movie.BLL.Services.Interfaces;

public interface IFilmService
{
    Task<IEnumerable<FilmGetDto>> GetFilms();
    Task<FilmGetDto> GetFilmByIdAsync(Guid id);
    Task<FilmGetDto> CreateFilmAsync(FilmCreateDto createDto);
    Task<FilmGetDto> UpdateFilmAsync(FilmUpdateDto updateDto);
    Task<bool> DeleteFilmAsync(Guid id);
}