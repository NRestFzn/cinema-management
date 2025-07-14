
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Helpers;
using CinemaManagement.Dto;
using Mapster;

namespace CinemaManagement.Controllers
{
    [Route("master/movie")]
    [ApiController]
    public class MasterMovieController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddMasterMovie([FromBody] CreateMasterMovieDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var genre = await _context.MasterGenre.FindAsync(formData.MasterGenreId);

            if (genre == null)
            {
                return ApiResponse.NotFound("Genre not found");
            }

            var newFormData = formData.Adapt<MasterMovie>();

            _context.MasterMovie.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<MasterMovieDto>();

            return ApiResponse.Created(nameof(GetMasterMovieById), new { id = newFormData.Id }, data, "Data succesfully created");

        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<MasterMovieDto>>>> GetAllMasterMovie()
        {
            var data = await _context.MasterMovie.ToListAsync();

            return ApiResponse.Ok(data.Adapt<List<MasterMovieDto>>(), "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<MasterMovieDetailDto>>> GetMasterMovieById(int Id)
        {
            var data = await _context.MasterMovie.Include(e => e.MasterGenre).FirstOrDefaultAsync(el => el.Id == Id);

            if (data == null)
            {
                return ApiResponse.NotFound("MasterMovie not found");
            }

            return ApiResponse.Ok(data.Adapt<MasterMovieDetailDto>(), "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateMasterMovie(int id, [FromBody] UpdateMasterMovieDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.MasterMovie.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound("MasterMovie not found");
            }

            var genre = await _context.MasterGenre.FindAsync(formData.MasterGenreId);

            if (genre == null)
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

            return ApiResponse.Ok("Data updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto>> DeleteMasterMovie(int id)
        {
            var data = await _context.MasterMovie.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound("MasterMovie not found");
            }

            _context.MasterMovie.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}