using Movie.BLL.DTOs.DirectorDTOs;

namespace Movie.BLL.Services.Interfaces;

public interface IDirectorService
{
    Task<IEnumerable<DirectorGetDto>> GetDirectors();
    Task<DirectorGetDto> GetDirectorByIdAsync(Guid id);
    Task<DirectorGetDto> CreateDirectorAsync(DirectorCreateDto createDto);
    Task<DirectorGetDto> UpdateDirectorAsync(DirectorUpdateDto updateDto);
    Task<bool> DeleteDirectorAsync(Guid id);
}