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
    public class StudioDto : BaseModel
    {
        public required string Name { get; set; }
        public required int Capacity { get; set; }
        public required int StudioTypeId { get; set; }
        public required int CinemaId { get; set; }
    }

    public class StudioDetailDto : StudioDto
    {
        [JsonPropertyOrder(97)]
        public required MasterStudioTypeDto StudioType { get; set; }
    }

    public class CreateStudioDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Studio name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Studio capacity is required")]
        public required int Capacity { get; set; }

        [Required(ErrorMessage = "Studio StudioTypeId is required")]
        public required int StudioTypeId { get; set; }

        [Required(ErrorMessage = "Cinema is required")]
        public required int CinemaId { get; set; }

    }

    public class UpdateStudioDto : CreateStudioDto { }
}