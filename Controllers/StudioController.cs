
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Helpers;
using CinemaManagement.Dto;
using Mapster;

namespace CinemaManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudioController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddStudio(CreateStudioDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var studioType = await _context.MasterStudioType.FindAsync(formData.StudioTypeId);
            var cinema = await _context.Cinema.FindAsync(formData.CinemaId);

            if (studioType == null) return ApiResponse.NotFound("MasterStudioType not found");
            if (cinema == null) return ApiResponse.NotFound("Cinema not found");

            var newFormData = formData.Adapt<Studio>();

            _context.Studio.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<StudioDto>();

            return ApiResponse.Created(nameof(GetStudioById), new { id = newFormData.Id }, data, "Data succesfully created");

        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<StudioDto>>>> GetAllStudio()
        {
            var data = await _context.Studio.ToListAsync();

            var results = data.Adapt<List<StudioDto>>();

            return ApiResponse.Ok(results, "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<StudioDetailDto>>> GetStudioById(int Id)
        {
            var studio = await _context.Studio.Include(e => e.StudioType).FirstOrDefaultAsync(el => el.Id == Id);

            if (studio == null)
            {
                return ApiResponse.NotFound("Studio not found");
            }

            var data = studio.Adapt<StudioDetailDto>();

            return ApiResponse.Ok(data, "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateStudio(int id, [FromBody] UpdateStudioDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.Studio.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound("Studio not found");
            }

            var studioType = await _context.MasterStudioType.FindAsync(formData.StudioTypeId);

            if (studioType == null)
            {
                return ApiResponse.NotFound("MasterStudioType not found");
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

            return ApiResponse.Ok("Data updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto>> DeleteStudio(int id)
        {
            var data = await _context.Studio.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound("Studio not found");
            }

            _context.Studio.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}