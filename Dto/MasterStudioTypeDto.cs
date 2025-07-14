using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CinemaManagement.Models;

namespace CinemaManagement.Dto
{
    public class MasterStudioTypeDto : BaseModel
    {
        public required string Name { get; set; }
    }

    public class MasterStudioTypeDetailDto : MasterStudioTypeDto
    {
        [JsonPropertyOrder(97)]
        public List<StudioDto> Studios { get; set; } = [];
    }

    public class CreateMasterStudioTypeDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "MasterStudioType name is required")]
        public required string Name { get; set; }
    }

    public class UpdateMasterStudioTypeDto : CreateMasterStudioTypeDto { }
}