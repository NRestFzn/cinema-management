
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Dto;
using CinemaManagement.Helpers;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CinemaManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto>> AddTransaction([FromBody] CreateTransactionFormData formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out var userId))
            {
                return ApiResponse.Unauthorized("Invalid user token.");
            }

            var schedule = await _context.MovieSchedule
                                         .FirstOrDefaultAsync(s => s.Id == formData.ScheduleId);

            if (schedule == null)
            {
                return ApiResponse.BadRequest([$"Schedule with ID {formData.ScheduleId} not found."]);
            }

            if (schedule.ScreeningDate < DateOnly.FromDateTime(DateTime.Now))
            {
                return ApiResponse.BadRequest(["Cannot book a schedule that has already passed."]);
            }

            var selectedChairs = await _context.Chair
                .Where(c => formData.ChairIds.Contains(c.Id) && c.StudioId == schedule.StudioId)
                .Include(c => c.ChairType)
                .ToListAsync();

            if (selectedChairs.Count != formData.ChairIds.Count)
            {
                return ApiResponse.BadRequest(["One or more selected chair IDs are invalid or do not belong to the selected studio."]);
            }

            var bookedChairIds = await _context.TransactionDetail
                .Where(td => td.Transaction.ScheduleId == formData.ScheduleId && formData.ChairIds.Contains(td.ChairId))
                .Select(td => td.ChairId)
                .ToListAsync();

            if (bookedChairIds.Any())
            {
                var bookedChairNumbers = selectedChairs.Where(c => bookedChairIds.Contains(c.Id)).Select(c => c.ChairNumber);
                return ApiResponse.BadRequest([$"The following chairs are already booked for this schedule: {string.Join(", ", bookedChairNumbers)}"]);
            }

            Voucher? voucher = null;
            if (formData.VoucherId.HasValue)
            {
                voucher = await _context.Voucher.FindAsync(formData.VoucherId.Value);
                if (voucher == null) return ApiResponse.BadRequest([$"Voucher not found."]);
                if (DateTime.Now < voucher.ValidDate || DateTime.Now > voucher.ExpiredDate) return ApiResponse.BadRequest(["Voucher is not valid at this time."]);
                if (voucher.Quota <= 0) return ApiResponse.BadRequest(["Voucher has run out of quota."]);
            }

            var chairTypeIds = selectedChairs.Select(c => c.ChairTypeId).Distinct().ToList();
            var relevantPriceRules = await _context.PriceRule
                .Where(pr => pr.StudioId == schedule.StudioId && chairTypeIds.Contains(pr.ChairTypeId))
                .ToListAsync();

            decimal originalAmount = 0;
            var chairPriceDetails = new Dictionary<int, decimal>();

            foreach (var chair in selectedChairs)
            {
                var dayOfWeek = schedule.ScreeningDate.DayOfWeek;
                var priceRule = relevantPriceRules.FirstOrDefault(pr =>
                    pr.ChairTypeId == chair.ChairTypeId &&
                    (pr.DayType.ToString() == dayOfWeek.ToString() || pr.IsWeekend == (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday))
                );

                if (priceRule == null)
                {
                    return ApiResponse.InternalServerError($"Price rule not found for Chair Type ID {chair.ChairTypeId}.");
                }

                chairPriceDetails[chair.Id] = priceRule.Price;
                originalAmount += priceRule.Price;
            }

            int voucherDiscount = voucher?.Discount ?? 0;
            decimal totalAmount = originalAmount - (originalAmount * voucherDiscount / 100);
            if (totalAmount < 0) totalAmount = 0;

            Transaction newTransaction = null;

            await using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                newTransaction = new Transaction
                {
                    UserId = userId,
                    PaymentMethodId = formData.PaymentMethodId,
                    ScheduleId = formData.ScheduleId,
                    VoucherId = formData.VoucherId,
                    TransactionCode = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper(),
                    Status = TransactionStatus.unpaid,
                    OriginalAmount = originalAmount,
                    VoucherDiscount = voucherDiscount,
                    TotalDiscount = voucherDiscount,
                    TotalAmount = totalAmount
                };

                _context.Transaction.Add(newTransaction);
                await _context.SaveChangesAsync();

                var transactionDetails = selectedChairs.Select(chair => new TransactionDetail
                {
                    TransactionId = newTransaction.Id,
                    ChairId = chair.Id,
                    Price = chairPriceDetails[chair.Id]
                }).ToList();

                _context.TransactionDetail.AddRange(transactionDetails);

                if (voucher != null)
                {
                    voucher.Quota--;
                    _context.Voucher.Update(voucher);
                }

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch (Exception)
            {
                await dbTransaction.RollbackAsync();
                return ApiResponse.InternalServerError("An error occurred while finalizing the transaction.");
            }

            return ApiResponse.Ok("Transaction created successfully.");
        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<TransactionDto>>>> GetAllTransaction()
        {
            var transactions = await _context.Transaction
                 .Include(t => t.MovieSchedule)
                     .ThenInclude(ms => ms.Movie)
                         .ThenInclude(m => m.MasterMovie)
                 .Include(t => t.TransactionDetail)
                 .OrderByDescending(t => t.CreatedAt)
                 .AsNoTracking()
                 .ToListAsync();

            var data = transactions.Adapt<List<TransactionDto>>();

            return ApiResponse.Ok(data, "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<TransactionDto>>> GetTransactionById(int Id)
        {
            var data = await _context.Transaction.FirstOrDefaultAsync(el => el.Id == Id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            return ApiResponse.Ok(data.Adapt<TransactionDto>(), "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdateTransaction(int id, [FromBody] UpdateTransactionDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.Transaction.FindAsync(id);

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
        public async Task<ActionResult<ApiResponseDto>> DeleteTransaction(int id)
        {
            var data = await _context.Transaction.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            _context.Transaction.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}