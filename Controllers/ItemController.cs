
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicApi.Data;
using BasicApi.Models;
using BasicApi.Dto;
using BasicApi.Helpers;

namespace BasicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(ApiDbContext context) : ControllerBase
    {
        private readonly ApiDbContext _context = context;

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<Item>>> AddItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest<Item>(errorMessages);
            }

            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return ApiResponse.Created(nameof(GetItemById), new { id = item.Id }, item, "Data succesfully created");
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<Item>>>> GetAllItem()
        {
            var daftarItem = await _context.Item.ToListAsync();

            return ApiResponse.Ok(daftarItem, "success get data");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<Item>>> GetItemById(int id)
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return ApiResponse.NotFound<Item>();
            }

            return ApiResponse.Ok(item, "Success get data");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Item.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}