using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CinemaManagement.Helpers;

namespace CinemaManagement.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        string errorMessage = "Authentication failed. Please provide a valid token.";

                        if (context.AuthenticateFailure != null)
                        {
                            switch (context.AuthenticateFailure)
                            {
                                case SecurityTokenExpiredException:
                                    errorMessage = "Your token has expired. Please login again.";
                                    break;

                                case SecurityTokenInvalidSignatureException:
                                    errorMessage = "The token signature is invalid.";
                                    break;

                                default:
                                    errorMessage = "An authentication error has occurred.";
                                    errorMessage += $" Details: {context.AuthenticateFailure.Message}";
                                    break;
                            }
                        }
                        else
                        {
                            errorMessage = "An authorization token is required.";
                        }

                        var apiResponseResult = ApiResponse.Unauthorized(errorMessage);
                        var payload = (apiResponseResult.Result as ObjectResult)?.Value;
                        var jsonResponse = JsonSerializer.Serialize(payload);

                        return context.Response.WriteAsync(jsonResponse);
                    }
                };
            });
        }
    }
}