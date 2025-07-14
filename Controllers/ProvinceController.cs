
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Dto;
using CinemaManagement.Helpers;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace CinemaManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProvinceController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]

        public async Task<ActionResult<ApiResponseDto>> AddProvince(CreateProvinceDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var newFormData = formData.Adapt<Province>();

            _context.Province.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<ProvinceDto>();

            return ApiResponse.Created(nameof(GetProvinceById), new { id = newFormData.Id }, data, "Data succesfully created");
        }

        [HttpGet]

        public async Task<ActionResult<GetDataResponseDto<List<ProvinceDto>>>> GetAllProvince()
        {
            var data = await _context.Province.ToListAsync();

            var results = data.Adapt<List<ProvinceDto>>();

            return ApiResponse.Ok(results, "success get data");
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<GetDataResponseDto<ProvinceDetailDto>>> GetProvinceById(int Id)
        {
            var data = await _context.Province.Include(e => e.Cities).FirstOrDefaultAsync(el => el.Id == Id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            var results = data.Adapt<ProvinceDetailDto>();

            return ApiResponse.Ok(results, "Success get data");
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<ApiResponseDto>> UpdateProvince(int id, [FromBody] UpdateProvinceDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.Province.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound();
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

        public async Task<ActionResult<ApiResponseDto>> DeleteProvince(int id)
        {
            var data = await _context.Province.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            _context.Province.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}