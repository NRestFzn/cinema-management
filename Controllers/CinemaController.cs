
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
    public class CinemaController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddCinema(CreateCinemaDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var city = await _context.City.FindAsync(formData.CityId);

            if (city == null)
            {
                return ApiResponse.NotFound("City not found");
            }

            var newFormData = formData.Adapt<Cinema>();

            _context.Cinema.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<CinemaDto>();

            return ApiResponse.Created(nameof(GetCinemaById), new { id = newFormData.Id }, data, "Data succesfully created");

        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<CinemaDto>>>> GetAllCinema()
        {
            var data = await _context.Cinema.ToListAsync();

            var results = data.Adapt<List<CinemaDto>>();

            return ApiResponse.Ok(results, "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<CinemaDetailDto>>> GetCinemaById(int Id)
        {
            var studio = await _context.Cinema.Include(e => e.City)
                                              .Include(e => e.Studios)
                                              .FirstOrDefaultAsync(el => el.Id == Id);

            if (studio == null)
            {
                return ApiResponse.NotFound("Cinema not found");
            }

            var data = studio.Adapt<CinemaDetailDto>();

            return ApiResponse.Ok(data, "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateCinema(int id, [FromBody] UpdateCinemaDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.Cinema.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound("Cinema not found");
            }

            var city = await _context.City.FindAsync(formData.CityId);

            if (city == null)
            {
                return ApiResponse.NotFound("City not found");
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
        public async Task<ActionResult<ApiResponseDto>> DeleteCinema(int id)
        {
            var data = await _context.Cinema.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound("Cinema not found");
            }

            _context.Cinema.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}