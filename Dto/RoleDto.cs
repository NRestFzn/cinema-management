using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.Dto
{
    public class RoleDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public class RoleWithUserDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public List<UserDto> Users { get; set; } = [];
    }
}