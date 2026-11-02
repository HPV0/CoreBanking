using CoreBanking.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Exceptions
{
    internal sealed class CustomValidationExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomValidationExceptionHandler> _logger;

        public CustomValidationExceptionHandler(ILogger<CustomValidationExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not CustomValidationException customValidationException)
            {
                return false;
            }

            _logger.LogError(
                customValidationException,
                "Exception occurred: {Message}",
                customValidationException.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Detail = customValidationException.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
