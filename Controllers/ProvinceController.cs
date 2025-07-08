
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Dto;
using CinemaManagement.Helpers;
using Mapster;

namespace CinemaManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<ProvinceDto>>> AddProvince(CreateProvinceDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest<ProvinceDto>(errorMessages);
            }

            var newFormData = formData.Adapt<Province>();

            _context.Province.Add(newFormData);

            await _context.SaveChangesAsync();

            var result = newFormData.Adapt<ProvinceDto>();

            return ApiResponse.Created(nameof(GetProvinceById), new { id = newFormData.Id }, result, "Data succesfully created");
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<ProvinceDto>>>> GetAllProvince()
        {
            var provinces = await _context.Province.ToListAsync();

            var results = provinces.Adapt<List<ProvinceDto>>();

            return ApiResponse.Ok(results, "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<ProvinceDetailDto>>> GetProvinceById(int Id)
        {
            var province = await _context.Province.Include(e => e.Cities).FirstOrDefaultAsync(el => el.Id == Id);

            if (province == null)
            {
                return ApiResponse.NotFound<ProvinceDetailDto>();
            }

            var results = province.Adapt<ProvinceDetailDto>();

            return ApiResponse.Ok(results, "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<ProvinceDto>>> UpdateProvince(int id, [FromBody] UpdateProvinceDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest<ProvinceDto>(errorMessages);
            }

            var existingProvince = await _context.Province.FindAsync(id);

            if (existingProvince == null)
            {
                return ApiResponse.NotFound<ProvinceDto>();
            }

            formData.Adapt(existingProvince);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<ProvinceDto>>> DeleteProvince(int id)
        {
            var province = await _context.Province.FindAsync(id);

            if (province == null)
            {
                return ApiResponse.NotFound<ProvinceDto>();
            }

            _context.Province.Remove(province);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok(province.Adapt<ProvinceDto>(), "Data deleted successfuly");
        }
    }
}