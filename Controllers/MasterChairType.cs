
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Dto;
using CinemaManagement.Helpers;
using Mapster;

namespace CinemaManagement.Controllers
{
    [Route("master/chairtype")]
    [ApiController]
    public class MasterChairTypeController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddMasterChairType([FromBody] CreateMasterChairTypeDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var newFormData = formData.Adapt<MasterChairType>();

            _context.MasterChairType.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<MasterChairTypeDto>();

            return ApiResponse.Created(nameof(GetMasterChairTypeById), new { id = newFormData.Id }, data, "Data succesfully created");
        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<MasterChairTypeDto>>>> GetAllMasterChairType()
        {
            var data = await _context.MasterChairType.ToListAsync();

            return ApiResponse.Ok(data.Adapt<List<MasterChairTypeDto>>(), "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<MasterChairTypeDetailDto>>> GetMasterChairTypeById(int Id)
        {
            var data = await _context.MasterChairType.Include(e => e.Chairs).FirstOrDefaultAsync(el => el.Id == Id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            return ApiResponse.Ok(data.Adapt<MasterChairTypeDetailDto>(), "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateMasterChairType(int id, [FromBody] UpdateMasterChairTypeDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.MasterChairType.FindAsync(id);

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

            return ApiResponse.Ok("Data successfully updated");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto>> DeleteMasterChairType(int id)
        {
            var data = await _context.MasterChairType.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            _context.MasterChairType.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}