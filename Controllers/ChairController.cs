
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Dto;
using CinemaManagement.Helpers;
using Mapster;

namespace CinemaManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChairController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddChair(CreateChairDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var chairType = await _context.MasterStudioType.FindAsync(formData.ChairTypeId);
            var studio = await _context.Studio.FindAsync(formData.StudioId);
            var existingChair = await _context.Chair.AnyAsync(e => e.ChairNumber == formData.ChairNumber);

            if (existingChair) return ApiResponse.BadRequest(["Chair number already exist"]);
            if (chairType == null) return ApiResponse.NotFound("ChairType not found");
            if (studio == null) return ApiResponse.NotFound("Studio not found");

            var chairs = await _context.Chair.Where(e => e.StudioId == formData.StudioId).ToListAsync();

            int totalChair = chairs.Count;

            if (totalChair == studio.Capacity) return ApiResponse.BadRequest(["can't add more chair due to max studio capacity"]);

            var newFormData = formData.Adapt<Chair>();

            _context.Chair.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<ChairDto>();

            return ApiResponse.Created(nameof(GetChairById), new { id = data.Id }, data, "Data succesfully created");
        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<ChairDto>>>> GetAllChair()
        {
            var data = await _context.Chair.ToListAsync();

            return ApiResponse.Ok<List<ChairDto>>(data.Adapt<List<ChairDto>>(), "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<ChairDetailDto>>> GetChairById(int Id)
        {
            var data = await _context.Chair.Include(e => e.ChairType).FirstOrDefaultAsync(el => el.Id == Id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            return ApiResponse.Ok(data.Adapt<ChairDetailDto>(), "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateChair(int id, [FromBody] UpdateChairDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.Chair.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            var chairType = await _context.MasterGenre.FindAsync(formData.ChairTypeId);

            if (chairType == null)
            {
                return ApiResponse.NotFound("Genre not found");
            }

            formData.Adapt(data);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return ApiResponse.Ok("Data successfully updated");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto>> DeleteChair(int id)
        {
            var data = await _context.Chair.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            _context.Chair.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}