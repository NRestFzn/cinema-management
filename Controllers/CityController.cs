
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
    public class CityController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddCity(CreateCityDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var findProvince = await _context.Province.FindAsync(formData.ProvinceId);

            if (findProvince == null)
            {
                return ApiResponse.NotFound("Province not found");
            }

            var newFormData = formData.Adapt<City>();

            _context.City.Add(newFormData);

            await _context.SaveChangesAsync();

            var result = newFormData.Adapt<CityDto>();

            return ApiResponse.Created(nameof(GetCityById), new { id = newFormData.Id }, result, "Data succesfully created");

        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<CityDto>>>> GetAllCity()
        {
            var cities = await _context.City.ToListAsync();

            var results = cities.Adapt<List<CityDto>>();

            return ApiResponse.Ok(results, "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<CityDetailDto>>> GetCityById(int Id)
        {
            var city = await _context.City.Include(e => e.Province).FirstOrDefaultAsync(el => el.Id == Id);

            if (city == null)
            {
                return ApiResponse.NotFound("City not found");
            }

            var results = city.Adapt<CityDetailDto>();

            return ApiResponse.Ok(results, "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateCity(int id, [FromBody] UpdateCityDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var existingCity = await _context.City.FindAsync(id);

            if (existingCity == null)
            {
                return ApiResponse.NotFound("City not found");
            }

            var findProvince = await _context.Province.FindAsync(formData.ProvinceId);

            if (findProvince == null)
            {
                return ApiResponse.NotFound("Province not found");
            }

            formData.Adapt(existingCity);

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
        public async Task<ActionResult<ApiResponseDto>> DeleteCity(int id)
        {
            var city = await _context.City.FindAsync(id);

            if (city == null)
            {
                return ApiResponse.NotFound("City not found");
            }

            _context.City.Remove(city);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}