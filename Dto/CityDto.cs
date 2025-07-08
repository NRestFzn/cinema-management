using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaManagement.Models;
using CinemaManagement.Dto;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CinemaManagement.Dto
{
    public class CityDto : BaseModel
    {
        public int ProvinceId { get; set; }
        public required string Name { get; set; }
    }

    public class CityDetailDto : CityDto
    {
        [JsonPropertyOrder(97)]
        public required ProvinceDto Province { get; set; }
    }

    public class CreateCityDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Required()]
        public required int ProvinceId { get; set; }
    }

    public class UpdateCityDto : CreateCityDto { }
}