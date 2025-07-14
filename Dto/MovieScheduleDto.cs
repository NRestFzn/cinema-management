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
    public class MovieScheduleDto : BaseModel
    {
        public int MovieId { get; set; }
        public int StudioId { get; set; }
        public DateOnly ScreeningDate { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
    }

    public class MovieScheduleDetailDto : MovieScheduleDto
    {
        [JsonPropertyOrder(97)]
        public required MovieDetailDto Movie { get; set; }

        [JsonPropertyOrder(96)]
        public required StudioDto Studio { get; set; }
    }

    public class CreateMovieScheduleDto
    {
        [Required(ErrorMessage = "Movie is required")]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Studio is required")]
        public int StudioId { get; set; }

        [Required(ErrorMessage = "Screening date is required")]
        public DateOnly ScreeningDate { get; set; }

        [Required(ErrorMessage = "Start hour is required")]
        public TimeOnly StartHour { get; set; }

        [Required(ErrorMessage = "End hour is required")]
        public TimeOnly EndHour { get; set; }
    }

    public class UpdateMovieScheduleDto : CreateMovieScheduleDto { }
}