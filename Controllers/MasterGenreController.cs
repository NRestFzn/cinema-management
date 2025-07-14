
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Dto;
using CinemaManagement.Helpers;
using Mapster;

namespace CinemaManagement.Controllers
{
    [Route("master/genre")]
    [ApiController]
    public class MasterGenreController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddMasterGenre([FromBody] CreateMasterGenreDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var newFormData = formData.Adapt<MasterGenre>();

            _context.MasterGenre.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<MasterGenreDto>();

            return ApiResponse.Created(nameof(GetMasterGenreById), new { id = newFormData.Id }, data, "Data succesfully created");
        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<MasterGenreDto>>>> GetAllMasterGenre()
        {
            var data = await _context.MasterGenre.ToListAsync();

            return ApiResponse.Ok(data.Adapt<List<MasterGenreDto>>(), "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<MasterGenreDetailDto>>> GetMasterGenreById(int Id)
        {
            var data = await _context.MasterGenre.Include(e => e.MasterMovies).FirstOrDefaultAsync(el => el.Id == Id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            return ApiResponse.Ok(data.Adapt<MasterGenreDetailDto>(), "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateMasterGenre(int id, [FromBody] UpdateMasterGenreDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.MasterGenre.FindAsync(id);

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
        public async Task<ActionResult<ApiResponseDto>> DeleteMasterGenre(int id)
        {
            var data = await _context.MasterGenre.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            _context.MasterGenre.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}