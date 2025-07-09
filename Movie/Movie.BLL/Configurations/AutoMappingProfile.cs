using AutoMapper;
using Movie.BLL.DTOs.ActorDTOs;
using Movie.DAL.Entities;

namespace Movie.BLL.Configurations;

public class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        CreateActorMaps();
    }

    private void CreateActorMaps()
    {
        CreateMap<ActorCreateDto, Actor>();
        CreateMap<ActorUpdateDto, Actor>();
        CreateMap<Actor, ActorGetDto>();
    }
}