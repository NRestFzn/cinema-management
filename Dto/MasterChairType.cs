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
    public class MasterChairTypeDto : BaseModel
    {
        public required string Name { get; set; }
    }

    public class MasterChairTypeDetailDto : MasterChairTypeDto
    {
        [JsonPropertyOrder(97)]
        public List<ChairDto> Chairs { get; set; } = [];
    }

    public class CreateMasterChairTypeDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "MasterChairType name is required")]
        public required string Name { get; set; }
    }

    public class UpdateMasterChairTypeDto : CreateMasterChairTypeDto { }
}