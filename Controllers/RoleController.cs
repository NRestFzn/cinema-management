
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicApi.Data;
using BasicApi.Models;
using BasicApi.Dto;
using BasicApi.Helpers;

namespace BasicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<Role>>> AddRole(Role item)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest<Role>(errorMessages);
            }

            _context.Role.Add(item);
            await _context.SaveChangesAsync();

            return ApiResponse.Created(nameof(GetRoleById), new { id = item.Id }, item, "Data succesfully created");
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<Role>>>> GetAllRole()
        {
            var Roles = await _context.Role.ToListAsync();

            return ApiResponse.Ok(Roles, "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<Role>>> GetRoleById(int id)
        {
            var item = await _context.Role.FindAsync(id);

            if (item == null)
            {
                return ApiResponse.NotFound<Role>();
            }

            return ApiResponse.Ok(item, "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, Role item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Role.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var item = await _context.Role.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Role.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}