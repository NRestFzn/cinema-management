using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CinemaManagement.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Fullname { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }

        [JsonIgnore]
        public required string Password { get; set; }
        public required int RoleId { get; set; }
    }

    public class UserDetailDto : UserDto
    {
        public required RoleDto Role { get; set; }
    }
}