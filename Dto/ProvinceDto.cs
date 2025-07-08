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
    public class ProvinceDto : BaseModel
    {
        public required string Name { get; set; }
    }

    public class ProvinceDetailDto : ProvinceDto
    {
        [JsonPropertyOrder(97)]
        public ICollection<CityDto> Cities { get; set; } = [];
    }

    public class CreateProvinceDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Province name is required")]
        public required string Name { get; set; }
    }

    public class UpdateProvinceDto : CreateProvinceDto { }
}