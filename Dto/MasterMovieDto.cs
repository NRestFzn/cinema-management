using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CinemaManagement.Dto;
using CinemaManagement.Models;

namespace CinemaManagement.Dto
{
    public class MasterMovieDto : BaseModel
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int MasterGenreId { get; set; }
        public required int Duration { get; set; }
        public required int AgeRating { get; set; }
    }

    public class MasterMovieDetailDto : MasterMovieDto
    {
        [JsonPropertyOrder(97)]
        public required MasterGenreDto MasterGenre { get; set; }
        // public ICollection<Movie> Movies { get; set; } = [];  
    }

    public class CreateMasterMovieDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int MasterGenreId { get; set; }
        public required int Duration { get; set; }
        public required int AgeRating { get; set; }
    }

    public class UpdateMasterMovieDto : CreateMasterMovieDto { }
}