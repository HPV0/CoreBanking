using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Exceptions
{
    internal sealed class FluentValidationExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<FluentValidationExceptionHandler> _logger;

        public FluentValidationExceptionHandler(ILogger<FluentValidationExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not ValidationException validationException)
            {
                return false;
            }

            _logger.LogError(
                validationException,
                "Exception occurred: {Message}",
                validationException.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
            };

            var errors = validationException.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .ToArray()
            );

            problemDetails.Extensions["errors"] = errors;


            httpContext.Response.StatusCode = problemDetails.Status.Value;


            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
