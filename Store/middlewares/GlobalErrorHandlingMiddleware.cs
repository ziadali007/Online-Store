using Azure.Core;
using Domain.Exceptions;
using Shared.ErrorModels;

namespace Store.middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode== StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPointAsync(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandlingErrorAsync(context, ex);
            }
        }

        private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new ErrorDetails()
            {
                ErrorMessage = ex.Message
            };

            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                ValidationException => HandlingValidationException((ValidationException) ex, response),
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"EndPoint With {context.Request.Path} not found"
            };
            await context.Response.WriteAsJsonAsync(response);
        }

        private static int HandlingValidationException(ValidationException ex, ErrorDetails response)
        {
            response.Errors = ex.Errors;
            return StatusCodes.Status400BadRequest;

        }
    }
}
