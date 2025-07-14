
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Helpers;
using CinemaManagement.Dto;
using Mapster;

namespace CinemaManagement.Controllers
{
    [Route("movie/schedule")]
    [ApiController]
    public class MovieScheduleController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddMovieSchedule(CreateMovieScheduleDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var movie = await _context.Movie.FindAsync(formData.MovieId);
            var studio = await _context.Studio.FindAsync(formData.StudioId);

            if (movie == null) return ApiResponse.NotFound("Movie not found");
            if (studio == null) return ApiResponse.NotFound("Studio not found");

            var newFormData = formData.Adapt<MovieSchedule>();

            _context.MovieSchedule.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<MovieScheduleDto>();

            return ApiResponse.Created(nameof(GetMovieScheduleById), new { id = newFormData.Id }, data, "Data succesfully created");

        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<MovieScheduleDto>>>> GetAllMovieSchedule()
        {
            var data = await _context.MovieSchedule.ToListAsync();

            var results = data.Adapt<List<MovieScheduleDto>>();

            return ApiResponse.Ok(results, "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<MovieScheduleDetailDto>>> GetMovieScheduleById(int Id)
        {
            var studio = await _context.MovieSchedule.Include(e => e.Movie)
                                                     .ThenInclude(e => e.MasterMovie)
                                                     .ThenInclude(e => e.MasterGenre)
                                                     .Include(e => e.Studio)
                                                     .FirstOrDefaultAsync(e => e.Id == Id);

            if (studio == null)
            {
                return ApiResponse.NotFound("MovieSchedule not found");
            }

            var data = studio.Adapt<MovieScheduleDetailDto>();

            return ApiResponse.Ok(data, "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateMovieSchedule(int id, [FromBody] UpdateMovieScheduleDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.MovieSchedule.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound("MovieSchedule not found");
            }

            var movie = await _context.Movie.FindAsync(formData.MovieId);
            var studio = await _context.Studio.FindAsync(formData.StudioId);

            if (movie == null) return ApiResponse.NotFound("Movie not found");
            if (studio == null) return ApiResponse.NotFound("Studio not found");

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
        public async Task<ActionResult<ApiResponseDto>> DeleteMovieSchedule(int id)
        {
            var data = await _context.MovieSchedule.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound("MovieSchedule not found");
            }

            _context.MovieSchedule.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}