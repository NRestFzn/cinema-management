using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CinemaManagement.Dto;

namespace CinemaManagement.Helpers
{
    public static class ApiResponse
    {
        public static ActionResult<ApiResponseDto> Ok<T>(T data, string? message = null)
        {
            message = string.IsNullOrEmpty(message) ? "success" : message;

            var response = new GetDataResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
            return new OkObjectResult(response);
        }

        public static ActionResult<ApiResponseDto> Ok(string? message = null)
        {
            message = string.IsNullOrEmpty(message) ? "success" : message;

            var response = new ApiResponseDto
            {
                Success = true,
                Message = message
            };
            return new OkObjectResult(response);
        }

        public static ActionResult<ApiResponseDto> Created<T>(string actionName, object routeValues, T data, string message)
        {
            var response = new GetDataResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data
            };

            return new CreatedAtActionResult(
                actionName: actionName,
                controllerName: null,
                routeValues: routeValues,
                value: response
            );
        }

        public static ActionResult NotFound(string? message = null)
        {
            message = string.IsNullOrEmpty(message) ? "Data not found" : message;

            var response = new ErrorResponseDto
            {
                Success = false,
                ErrorCode = StatusCodes.Status404NotFound,
                Errors = [message],
            };
            return new NotFoundObjectResult(response);
        }

        public static ActionResult Forbidden(string? message = null)
        {
            message = string.IsNullOrEmpty(message) ? "you dont have permission" : message;

            var response = new ErrorResponseDto
            {
                Success = false,
                ErrorCode = StatusCodes.Status404NotFound,
                Errors = [message],
            };
            return new NotFoundObjectResult(response);
        }

        public static ActionResult<ApiResponseDto> BadRequest(List<String> messages)
        {
            var response = new ErrorResponseDto
            {
                Success = false,
                ErrorCode = StatusCodes.Status400BadRequest,
                Errors = messages,
            };
            return new BadRequestObjectResult(response);
        }

        public static ActionResult<ApiResponseDto> InternalServerError(string message)
        {
            var response = new ErrorResponseDto
            {
                Success = false,
                Message = "Internal server error.",
                ErrorCode = StatusCodes.Status500InternalServerError,
                Errors = [message]
            };
            return new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public static ActionResult<ApiResponseDto> Unauthorized(string message)
        {
            var response = new ErrorResponseDto
            {
                Success = false,
                Message = message,
                ErrorCode = StatusCodes.Status401Unauthorized,
            };
            return new UnauthorizedObjectResult(response);
        }
    }
}