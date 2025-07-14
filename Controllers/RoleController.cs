
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using CinemaManagement.Data;
// using CinemaManagement.Models;
// using CinemaManagement.Dto;
// using CinemaManagement.Helpers;

// namespace CinemaManagement.Controllers
// {
//     [Route("[controller]")]
//     [ApiController]
//     public class RoleController(ApiDbContext context) : ControllerBase
//     {
//         private readonly ApiDbContext _context = context;

//         [HttpPost]
//         public async Task<ActionResult<ApiResponseDto<Role>>> AddRole(Role item)
//         {
//             if (!ModelState.IsValid)
//             {
//                 var errorMessages = ModelState.Values
//                     .SelectMany(v => v.Errors)
//                     .Select(e => e.ErrorMessage).ToList();

//                 return ApiResponse.BadRequest<Role>(errorMessages);
//             }

//             _context.Role.Add(item);
//             await _context.SaveChangesAsync();

//             return ApiResponse.Created(nameof(GetRoleById), new { id = item.Id }, item, "Data succesfully created");
//         }

//         [HttpGet]
//         public async Task<ActionResult<ApiResponseDto<List<RoleWithUserDto>>>> GetAllRole()
//         {
//             var Roles = await _context.Role.Include(e => e.Users).ToListAsync();

//             var results = Roles.Select(e => new RoleWithUserDto
//             {
//                 Id = e.Id,
//                 Name = e.Name,
//                 Users = [.. e.Users.Select(el => new UserDto
//                 {
//                     Id = el.Id,
//                     Fullname = el.Fullname,
//                     Email = el.Email,
//                     RoleId = el.RoleId
//                 })]
//             }).ToList();

//             return ApiResponse.Ok(results, "success get data");
//         }

//         [HttpGet("{id}")]
//         public async Task<ActionResult<ApiResponseDto<RoleWithUserDto>>> GetRoleById(int Id)
//         {
//             var role = await _context.Role.Include(e => e.Users).FirstOrDefaultAsync(el => el.Id == Id);

//             if (role == null)
//             {
//                 return ApiResponse.NotFound<RoleWithUserDto>();
//             }

//             var results = new RoleWithUserDto
//             {
//                 Id = role.Id,
//                 Name = role.Name,
//                 Users = [.. role.Users.Select(e => new UserDto
//                 {
//                      Id = e.Id,
//                      Fullname = e.Fullname,
//                      Email = e.Email,
//                      RoleId =e.RoleId
//                 })]
//             };

//             return ApiResponse.Ok(results, "Success get data");
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateRole(int id, Role item)
//         {
//             if (id != item.Id)
//             {
//                 return BadRequest();
//             }

//             _context.Entry(item).State = EntityState.Modified;

//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!_context.Role.Any(e => e.Id == id))
//                 {
//                     return NotFound();
//                 }
//                 else
//                 {
//                     throw;
//                 }
//             }

//             return NoContent();
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteRole(int id)
//         {
//             var item = await _context.Role.FindAsync(id);
//             if (item == null)
//             {
//                 return NotFound();
//             }

//             _context.Role.Remove(item);
//             await _context.SaveChangesAsync();

//             return Ok();
//         }
//     }
// }