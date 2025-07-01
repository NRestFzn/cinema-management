using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BasicApi.Models
{
    public class Role : BaseModel
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; } = [];
    }
}