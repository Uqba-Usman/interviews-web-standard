using Microsoft.AspNetCore.Mvc;

namespace api
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Forward the request to the next middleware component in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "An unhandled exception has occurred.");

                
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred.",
                    Detail = ex.Message, 
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }

}
