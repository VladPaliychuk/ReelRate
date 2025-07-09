using Movie.BLL.DTOs.GenreDTOs;

namespace Movie.BLL.Services.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreGetDto>> GetGenres();
    Task<GenreGetDto> GetGenreByIdAsync(Guid id);
    Task<GenreGetDto> CreateGenreAsync(GenreCreateDto createDto);
    Task<GenreGetDto> UpdateGenreAsync(GenreUpdateDto updateDto);
    Task<bool> DeleteGenreAsync(Guid id);
}