using Movie.BLL.DTOs.ActorDTOs;
using Movie.BLL.DTOs.DirectorDTOs;
using Movie.BLL.DTOs.GenreDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.BLL.DTOs.FilmDTOs
{
    public class FilmWithRelationsCreateDto
    {
        public FilmCreateDto Film { get; set; } = null!;
        public List<ActorCreateDto> Actors { get; set; } = new();
        public List<GenreCreateDto> Genres { get; set; } = new();
        public List<DirectorCreateDto> Directors { get; set; } = new();
    }
}   
