
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Dto;
using CinemaManagement.Helpers;
using Mapster;

namespace CinemaManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentMethodController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto>> AddPaymentMethod([FromBody] CreatePaymentMethodDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var newFormData = formData.Adapt<PaymentMethod>();

            _context.PaymentMethod.Add(newFormData);

            await _context.SaveChangesAsync();

            var data = newFormData.Adapt<PaymentMethodDto>();

            return ApiResponse.Created(nameof(GetPaymentMethodById), new { id = newFormData.Id }, data, "Data succesfully created");
        }

        [HttpGet]
        public async Task<ActionResult<GetDataResponseDto<List<PaymentMethodDto>>>> GetAllPaymentMethod()
        {
            var data = await _context.PaymentMethod.ToListAsync();

            return ApiResponse.Ok(data.Adapt<List<PaymentMethodDto>>(), "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDataResponseDto<PaymentMethodDetailDto>>> GetPaymentMethodById(int Id)
        {
            var data = await _context.PaymentMethod.Include(e => e.Transactions).FirstOrDefaultAsync(el => el.Id == Id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            return ApiResponse.Ok(data.Adapt<PaymentMethodDetailDto>(), "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto>> UpdatePaymentMethod(int id, [FromBody] UpdatePaymentMethodDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return ApiResponse.BadRequest(errorMessages);
            }

            var data = await _context.PaymentMethod.FindAsync(id);

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
        public async Task<ActionResult<ApiResponseDto>> DeletePaymentMethod(int id)
        {
            var data = await _context.PaymentMethod.FindAsync(id);

            if (data == null)
            {
                return ApiResponse.NotFound();
            }

            _context.PaymentMethod.Remove(data);

            await _context.SaveChangesAsync();

            return ApiResponse.Ok("Data deleted successfuly");
        }
    }
}