using AutoMapper;
using Movie.BLL.DTOs.ActorDTOs;
using Movie.BLL.DTOs.DirectorDTOs;
using Movie.BLL.DTOs.FilmDTOs;
using Movie.BLL.DTOs.GenreDTOs;
using Movie.BLL.DTOs.RatingDTOs;
using Movie.DAL.Entities;

namespace Movie.BLL.Configurations;

public class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        CreateActorMaps();
        CreateDirectorMaps();
        CreateFilmMaps();
        CreateGenreMaps();
        CreateRatingMaps();
    }

    private void CreateActorMaps()
    {
        CreateMap<ActorCreateDto, Actor>();
        CreateMap<ActorUpdateDto, Actor>();
        CreateMap<Actor, ActorGetDto>();
    }
    
    private void CreateDirectorMaps()
    {
        CreateMap<DirectorCreateDto, Director>();
        CreateMap<DirectorUpdateDto, Director>();
        CreateMap<Director, DirectorGetDto>();
    }
    
    private void CreateFilmMaps()
    {
        CreateMap<FilmCreateDto, Film>();
        CreateMap<FilmUpdateDto, Film>();
        CreateMap<Film, FilmGetDto>()
            .ForMember(dest => dest.Rating,
                opt => opt.MapFrom(src => src.Rating != null ? (double?)src.Rating.Score : null));
    }
    
    private void CreateGenreMaps()
    {
        CreateMap<GenreCreateDto, Genre>();
        CreateMap<GenreUpdateDto, Genre>();
        CreateMap<Genre, GenreGetDto>();
    }
    
    private void CreateRatingMaps()
    {
        CreateMap<RatingCreateDto, Rating>();
        CreateMap<Rating, RatingGetDto>();
    }
}