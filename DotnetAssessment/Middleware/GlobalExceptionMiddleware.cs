
using Application.Abstractions.Services;

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
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, context.Request.Path, CancellationToken.None);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
            }
        }
    }


}