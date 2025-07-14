
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Dto;
using CinemaManagement.Helpers;
using Mapster;

namespace CinemaManagement.Controllers
{
    [Route("master/studiotype")]
    [ApiController]
    public class MasterStudioTypeController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddMasterStudioType([FromBody] CreateMasterStudioTypeDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var newFormData = formData.Adapt<MasterStudioType>();

            _context.MasterStudioType.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<MasterStudioTypeDto>();

            return ApiResponse.Created(nameof(GetMasterStudioTypeById), new { id = newFormData.Id }, data, "Data succesfully created");
        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<MasterStudioTypeDto>>>> GetAllMasterStudioType()
        {
            var data = await _context.MasterStudioType.ToListAsync();

            return ApiResponse.Ok(data.Adapt<List<MasterStudioTypeDto>>(), "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<MasterStudioTypeDetailDto>>> GetMasterStudioTypeById(int Id)
        {
            var data = await _context.MasterStudioType.Include(e => e.Studios).FirstOrDefaultAsync(el => el.Id == Id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            return ApiResponse.Ok(data.Adapt<MasterStudioTypeDetailDto>(), "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateMasterStudioType(int id, [FromBody] UpdateMasterStudioTypeDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.MasterStudioType.FindAsync(id);

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
        public async Task<ActionResult<ApiResponseDto>> DeleteMasterStudioType(int id)
        {
            var data = await _context.MasterStudioType.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            _context.MasterStudioType.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}