
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
    public class MovieController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddMovie(CreateMovieDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var masterMovie = await _context.MasterMovie.FindAsync(formData.MasterMovieId);
            var cinema = await _context.Cinema.FindAsync(formData.CinemaId);

            if (masterMovie == null) return ApiResponse.NotFound("MasterMovie not found");
            if (cinema == null) return ApiResponse.NotFound("Cinema not found");

            var newFormData = formData.Adapt<Movie>();

            _context.Movie.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<MovieDto>();

            return ApiResponse.Created(nameof(GetMovieById), new { id = newFormData.Id }, data, "Data succesfully created");

        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<MovieDto>>>> GetAllMovie()
        {
            var data = await _context.Movie.ToListAsync();

            var results = data.Adapt<List<MovieDto>>();

            return ApiResponse.Ok(results, "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<MovieDetailDto>>> GetMovieById(int Id)
        {
            var studio = await _context.Movie.Include(e => e.MasterMovie).ThenInclude(e => e.MasterGenre)
                                             .Include(e => e.Cinema)
                                             .FirstOrDefaultAsync(el => el.Id == Id);

            if (studio == null)
            {
                return ApiResponse.NotFound("Movie not found");
            }

            var data = studio.Adapt<MovieDetailDto>();

            return ApiResponse.Ok(data, "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateMovie(int id, [FromBody] UpdateMovieDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.Movie.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound("Movie not found");
            }

            var masterMovie = await _context.MasterMovie.FindAsync(formData.MasterMovieId);

            if (masterMovie == null)
            {
                return ApiResponse.NotFound("MasterMovie not found");
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
        public async Task<ActionResult<ApiResponseDto>> DeleteMovie(int id)
        {
            var data = await _context.Movie.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound("Movie not found");
            }

            _context.Movie.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}