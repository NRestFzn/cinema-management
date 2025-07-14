
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
    // [Authorize(Roles = "Admin")]
    public class ReportController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpGet("sales")]

        public async Task<IActionResult> GetSalesReport(
    [FromQuery] string reportType,
    [FromQuery] int year,
    [FromQuery] int? month,
    [FromQuery] int? day)
        {
            string period;
            var query = _context.Transaction
                                .Where(t => t.Status == TransactionStatus.paid);

            if (reportType.Equals("daily", StringComparison.OrdinalIgnoreCase))
            {
                if (!day.HasValue || !month.HasValue)
                {
                    return BadRequest(new { message = "'day' and 'month' parameters are required for daily reports." });
                }
                var reportDate = new DateOnly(year, month.Value, day.Value);
                query = query.Where(t => DateOnly.FromDateTime(t.CreatedAt) == reportDate);
                period = reportDate.ToString("dd MMMM yyyy");
            }
            else if (reportType.Equals("monthly", StringComparison.OrdinalIgnoreCase))
            {
                if (!month.HasValue)
                {
                    return BadRequest(new { message = "'month' parameter is required for monthly reports." });
                }
                query = query.Where(t => t.CreatedAt.Year == year && t.CreatedAt.Month == month.Value);
                period = new DateOnly(year, month.Value, 1).ToString("MMMM yyyy");
            }
            else
            {
                return BadRequest(new { message = "Invalid reportType. Use 'daily' or 'monthly'." });
            }

            var reportDetails = await query
                .Include(t => t.MovieSchedule.Movie.MasterMovie)
                .Include(t => t.MovieSchedule.Studio)
                .GroupBy(t => new
                {
                    MovieTitle = t.MovieSchedule.Movie.MasterMovie.Title,
                    StudioName = t.MovieSchedule.Studio.Name
                })
                .Select(g => new SalesReportItemDto
                {
                    MovieTitle = g.Key.MovieTitle,
                    StudioName = g.Key.StudioName,
                    TicketsSold = g.SelectMany(t => t.TransactionDetail).Count(),
                    TotalRevenue = g.Sum(t => t.TotalAmount)
                })
                .OrderByDescending(r => r.TotalRevenue)
                .ToListAsync();

            var finalReport = new SalesReportDto
            {
                ReportType = reportType,
                Period = period,
                Details = reportDetails,
                GrandTotalRevenue = reportDetails.Sum(r => r.TotalRevenue),
                GrandTotalTickets = reportDetails.Sum(r => r.TicketsSold)
            };

            return Ok(finalReport);
        }
    }
}