using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Assessment.Shared
{
    /// <summary>
    /// Hides error 500 details from clients.
    /// </summary>
    public class CustomExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> logger;

        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            this.logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status500InternalServerError)
            {
                logger.LogCritical(exception, "Internal Server Error");

                // Generic error response
                var errorResponse = new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred. Please try again later."
                };

                await httpContext.Response.WriteAsJsonAsync(errorResponse);

                return true;
            }

            logger.LogError(exception, "Uncaught Exception");

            return false;
        }
    }
}
