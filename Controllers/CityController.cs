
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Helpers;
using CinemaManagement.Dto;
using Mapster;

namespace CinemaManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<CityDto>>> AddCity(CreateCityDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest<CityDto>(errorMessages);
            }

            var findProvince = await _context.Province.FindAsync(formData.ProvinceId);

            if (findProvince == null)
            {
                return ApiResponse.NotFound<CityDto>("Province not found");
            }

            var newFormData = formData.Adapt<City>();

            _context.City.Add(newFormData);

            await _context.SaveChangesAsync();

            var result = newFormData.Adapt<CityDto>();

            return ApiResponse.Created(nameof(GetCityById), new { id = newFormData.Id }, result, "Data succesfully created");

        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<CityDto>>>> GetAllCity()
        {
            var provinces = await _context.City.ToListAsync();

            var results = provinces.Adapt<List<CityDto>>();

            return ApiResponse.Ok(results, "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<CityDetailDto>>> GetCityById(int Id)
        {
            var city = await _context.City.Include(e => e.Province).FirstOrDefaultAsync(el => el.Id == Id);

            if (city == null)
            {
                return ApiResponse.NotFound<CityDetailDto>("City not found");
            }

            var results = city.Adapt<CityDetailDto>();

            return ApiResponse.Ok(results, "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<CityDto>>> UpdateCity(int id, [FromBody] UpdateCityDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest<CityDto>(errorMessages);
            }

            var existingCity = await _context.City.FindAsync(id);

            if (existingCity == null)
            {
                return ApiResponse.NotFound<CityDto>("City not found");
            }

            var findProvince = await _context.Province.FindAsync(formData.ProvinceId);

            if (findProvince == null)
            {
                return ApiResponse.NotFound<CityDto>("Province not found");
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

            return ApiResponse.Ok(existingCity.Adapt<CityDto>(), "Data updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<CityDto>>> DeleteCity(int id)
        {
            var province = await _context.City.FindAsync(id);

            if (province == null)
            {
                return ApiResponse.NotFound<CityDto>("City not found");
            }

            _context.City.Remove(province);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok(province.Adapt<CityDto>(), "Data deleted successfuly");
        }
    }
}