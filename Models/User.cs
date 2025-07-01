using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BasicApi.Models
{
    public class User : BaseModel
    {
        [Required(ErrorMessage = "Fullname is required")]
        public required string Fullname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        public required int RoleId { get; set; }

        public Role Role { get; set; }
    }
}