using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using CinemaManagement.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using CinemaManagement.Dto;
namespace CinemaManagement.Middlewares
{

    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception has occurred: {Message}", exception.Message);

            ActionResult<ApiResponseDto> errorResponse;

            if (exception is DbUpdateException dbUpdateException &&
                dbUpdateException.InnerException is MySqlException mySqlException)
            {
                errorResponse = mySqlException.Number switch
                {
                    1452 => ApiResponse.BadRequest([$"Invalid foreign key. The provided ID does not exist in the related table."]),

                    1062 => ApiResponse.BadRequest([$"Duplicate entry. The value already exists and must be unique."]),

                    _ => ApiResponse.InternalServerError("A database error occurred.")
                };
            }
            else
            {
                errorResponse = ApiResponse.InternalServerError("An unexpected server error has occurred.");
            }

            var objectResult = errorResponse.Result as ObjectResult;

            httpContext.Response.StatusCode = objectResult?.StatusCode ?? 500;

            await httpContext.Response.WriteAsJsonAsync(objectResult?.Value, cancellationToken);

            return true;

        }
    }

}