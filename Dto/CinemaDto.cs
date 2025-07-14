using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CinemaManagement.Models;

namespace CinemaManagement.Dto
{
    public class CinemaDto : BaseModel
    {
        public required string Name { get; set; }
        public int CityId { get; set; }
        public required string DetailLocation { get; set; }
    }

    public class CinemaDetailDto : CinemaDto
    {
        [JsonPropertyOrder(97)]
        public required CityDto City { get; set; }

        [JsonPropertyOrder(96)]
        public ICollection<StudioDto> Studios { get; set; } = [];
        // public ICollection<OperatingHour> OperatingHours = [];
    }

    public class CreateCinemaDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cinema name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "City is required")]
        public int CityId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Detail Location is required")]
        public required string DetailLocation { get; set; }
    }

    public class UpdateCinemaDto : CreateCinemaDto { }
}