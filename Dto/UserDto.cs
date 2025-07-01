using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicApi.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public RoleDto? Role { get; set; }
    }
}