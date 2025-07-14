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
    public class ChairDto : BaseModel
    {
        public required string ChairNumber { get; set; }
        public required ChairStatus ChairStatus { get; set; }
        public int StudioId { get; set; }
        public int ChairTypeId { get; set; }
    }

    public class ChairDetailDto : ChairDto
    {
        [JsonPropertyOrder(97)]
        public required MasterChairTypeDto ChairType { get; set; }
    }

    public class CreateChairDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Chair number name is required")]
        public required string ChairNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chair number is required")]
        [EnumDataType(typeof(ChairStatus), ErrorMessage = "Status must be 'available', 'ordered', 'damaged'")]
        public required ChairStatus ChairStatus { get; set; }

        [Required(ErrorMessage = "StudioId is required")]
        public int StudioId { get; set; }

        [Required(ErrorMessage = "MasterChairType id is required")]
        public int ChairTypeId { get; set; }
    }

    public class UpdateChairDto : CreateChairDto { }
}