using Movie.BLL.DTOs.ActorDTOs;

namespace Movie.BLL.Services.Interfaces;

public interface IActorService
{
    Task<IEnumerable<ActorGetDto>> GetActors();
    Task<ActorGetDto?> GetActorByIdAsync(Guid id);
    Task<ActorGetDto> CreateActorAsync(ActorCreateDto createDto);
    Task<ActorGetDto?> UpdateActorAsync(ActorUpdateDto updateDto);
    Task<bool> DeleteActorAsync(Guid id);
}