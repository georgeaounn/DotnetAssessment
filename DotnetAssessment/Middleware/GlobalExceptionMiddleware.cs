using Application.Common;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DotnetAssessment.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                var errorMessages = ex.Errors.Select(e => e.ErrorMessage).ToList();
                var result = Result.Failure(errorMessages);
                await context.Response.WriteAsJsonAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "An unexpected error occurred. Path: {Path}, Method: {Method}", 
                    context.Request.Path, 
                    context.Request.Method);
                
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var result = Result.Failure("An unexpected error occurred.");
                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}