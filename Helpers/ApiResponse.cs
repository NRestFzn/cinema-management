using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BasicApi.Dto;

namespace BasicApi.Helpers
{
    public static class ApiResponse
    {
        public static ActionResult<ApiResponseDto<T>> Ok<T>(T data, string message)
        {
            var response = new ApiResponseDto<T>
            {
                success = true,
                message = message,
                data = data
            };
            return new OkObjectResult(response);
        }

        public static ActionResult<ApiResponseDto<T>> Created<T>(string actionName, object routeValues, T data, string message)
        {
            var response = new ApiResponseDto<T>
            {
                success = true,
                message = message,
                data = data
            };

            return new CreatedAtActionResult(
                actionName: actionName,
                controllerName: null,
                routeValues: routeValues,
                value: response
            );
        }

        public static ActionResult<ApiResponseDto<T>> NotFound<T>()
        {
            var response = new ApiResponseDto<object>
            {
                success = false,
                data = default(T),
                errorCode = StatusCodes.Status404NotFound,
                errors = ["Data not found"],
            };
            return new NotFoundObjectResult(response);
        }

        public static ActionResult<ApiResponseDto<T>> BadRequest<T>(List<String> messages)
        {
            var response = new ApiResponseDto<T>
            {
                success = false,
                errorCode = StatusCodes.Status400BadRequest,
                errors = messages,
            };
            return new BadRequestObjectResult(response);
        }

        public static ActionResult<ApiResponseDto<object>> InternalServerError(string message)
        {
            var response = new ApiResponseDto<object>
            {
                success = false,
                message = "Internal server error.",
                errorCode = StatusCodes.Status500InternalServerError,
                errors = [message]
            };
            return new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}