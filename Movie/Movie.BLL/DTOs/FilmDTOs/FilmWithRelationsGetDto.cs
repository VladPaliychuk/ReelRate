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
    public class FilmWithRelationsGetDto
    {
        public FilmGetDto Film { get; set; } = null!;
        public List<ActorGetDto> Actors { get; set; } = new();
        public List<GenreGetDto> Genres { get; set; } = new();
        public List<DirectorGetDto> Directors { get; set; } = new();
    }
}
