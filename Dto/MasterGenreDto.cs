using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CinemaManagement.Dto;
using CinemaManagement.Models;

namespace CinemaManagement.Dto
{
    public class MasterGenreDto : BaseModel
    {
        public required string Name { get; set; }
    }

    public class MasterGenreDetailDto : MasterGenreDto
    {
        [JsonPropertyOrder(97)]
        public List<MasterMovieDto> MasterMovies { get; set; } = [];
    }

    public class CreateMasterGenreDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "MasterGenre name is required")]
        public required string Name { get; set; }
    }

    public class UpdateMasterGenreDto : CreateMasterGenreDto { }
}