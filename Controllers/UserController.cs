
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Dto;
using CinemaManagement.Helpers;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CinemaManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        // [HttpPost]
        // public async Task<ActionResult<ApiResponseDto<User>>> AddUser(User item)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         var errorMessages = ModelState.Values
        //             .SelectMany(v => v.Errors)
        //             .Select(e => e.ErrorMessage).ToList();

        //         return ApiResponse.BadRequest<User>(errorMessages);
        //     }

        //     _context.User.Add(item);
        //     await _context.SaveChangesAsync();

        //     return ApiResponse.Created(nameof(GetUserById), new { id = item.Id }, item, "Data succesfully created");
        // }

        // [HttpGet]
        // public async Task<ActionResult<ApiResponseDto<List<UserDto>>>> GetAllUser()
        // {
        //     var users = await _context.User.Include(u => u.Role).ToListAsync();

        //     var results = users.Select(e => new UserDto
        //     {
        //         Id = e.Id,
        //         Fullname = e.Fullname,
        //         Email = e.Email,
        //         Password = e.Password,
        //         RoleId = e.RoleId,
        //         Role = new RoleDto
        //         {
        //             Id = e.Role.Id,
        //             Name = e.Role.Name
        //         },
        //     }).ToList();

        //     return ApiResponse.Ok(results, "success get data");
        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<UserDto>>> GetUserById(int id)
        {
            var user = await _context.User.Include(u => u.RoleId == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return ApiResponse.NotFound("User not found");
            }

            return ApiResponse.Ok(user.Adapt<UserDto>(), "Success get data");
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetMyProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userName = User.FindFirst("fullname")?.Value;

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Id = userId,
                Name = userName,
                Role = userRole
            });
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateUser(int id, User item)
        // {
        //     if (id != item.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(item).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!_context.User.Any(e => e.Id == id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteUser(int id)
        // {
        //     var item = await _context.User.FindAsync(id);
        //     if (item == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.User.Remove(item);
        //     await _context.SaveChangesAsync();

        //     return Ok();
        // }
    }
}