using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CinemaManagement.Models;
using CinemaManagement.Dto;

namespace CinemaManagement.Dto
{
    public class MovieDto : BaseModel
    {
        public int MasterMovieId { get; set; }
        public int CinemaId { get; set; }
        public MovieStatus Status { get; set; }
    }

    public class MovieDetailDto : MovieDto
    {
        [JsonPropertyOrder(97)]
        public required MasterMovieDetailDto MasterMovie { get; set; }

        [JsonPropertyOrder(96)]
        public required CinemaDto Cinema { get; set; }
    }

    public class CreateMovieDto
    {
        [Required(ErrorMessage = "MasterMovie is required")]
        public int MasterMovieId { get; set; }

        [Required(ErrorMessage = "Cinema is required")]
        public int CinemaId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Movie status is required")]
        [EnumDataType(typeof(MovieStatus), ErrorMessage = "Status must be 'upcoming' or 'nowshowing'")]
        public MovieStatus Status { get; set; }
    }

    public class UpdateMovieDto : CreateMovieDto { }
}