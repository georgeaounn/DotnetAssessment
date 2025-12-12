
using Application.Abstractions.Services;
using Application.Common;
using FluentValidation;
using System.Linq;

namespace DotnetAssessment.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IErrorLogger _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, IErrorLogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            } catch(ValidationException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                var errorMessages = ex.Errors.Select(e => e.ErrorMessage).ToList();
                var result = Result.Failure(errorMessages);
                await context.Response.WriteAsJsonAsync(result);
            } catch(Exception ex)
            {
                await _logger.LogErrorAsync(ex, context.Request.Path, CancellationToken.None);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var result = Result.Failure("An unexpected error occurred.");
                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}