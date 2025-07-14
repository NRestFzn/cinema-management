using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CinemaManagement.Data;
using CinemaManagement.Dto;
using CinemaManagement.Helpers;
using CinemaManagement.Models;
using Mapster;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CinemaManagement.Controllers;

namespace CinemaManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(ApiDbContext context, IConfiguration configuration) : ControllerBase
    {
        private readonly ApiDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("signup")]
        public async Task<ActionResult<ApiResponseDto>> SignUp([FromBody] SignUpDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var duplicateEmail = await _context.User.FirstOrDefaultAsync(e => e.Email == formData.Email);

            if (duplicateEmail != null) return ApiResponse.BadRequest(["Email already used"]);

            var duplicatePhoneNumber = await _context.User.FirstOrDefaultAsync(e => e.PhoneNumber == formData.PhoneNumber);

            if (duplicatePhoneNumber != null) return ApiResponse.BadRequest(["Phone number already used"]);

            var defaultRole = await _context.Role.FirstOrDefaultAsync(e => e.Name == "member");

            if (defaultRole == null) return ApiResponse.NotFound("Role not found");

            var newFormData = formData.Adapt<User>();

            newFormData.RoleId = defaultRole.Id;
            newFormData.Password = BCrypt.Net.BCrypt.HashPassword(formData.Password);

            _context.User.Add(newFormData);

            await _context.SaveChangesAsync();

            var token = this.GenerateUserToken(
                newFormData.Id,
                newFormData.Email,
                newFormData.Fullname,
                defaultRole.Name
            );

            var response = new AuthSuccessResponseDto
            {
                Token = token,
                UserId = newFormData.Id
            };

            return ApiResponse.AuthOk(token, newFormData.Id, "Account succesfully registered");
        }

        [HttpPost("signin")]
        public async Task<ActionResult<ApiResponseDto>> SignIn([FromBody] SignInDto formData)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return ApiResponse.BadRequest(errorMessages);
            }

            var findUser = await _context.User.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == formData.Email);

            if (findUser == null) return ApiResponse.NotFound("User not found");

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(formData.Password, findUser.Password);

            if (!verifyPassword) return ApiResponse.BadRequest(["Incorrect password or email"]);

            var token = this.GenerateUserToken(
                findUser.Id,
                findUser.Email,
                findUser.Fullname,
                findUser.Role.Name
            );

            return ApiResponse.AuthOk(token, findUser.Id, "Login success");
        }

        private string GenerateUserToken(int Id, string Email, string Fullname, string Role)
        {
            var claims = new List<Claim>
            {
                new("fullname", Fullname),
                new("email", Email),
                new("role", Role),
                new("sub", Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                    )
                );

            return token;
        }
    }
}