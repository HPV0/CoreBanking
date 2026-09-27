using CoreBanking.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CoreBanking.API.Exceptions
{
    internal sealed class BusinessLogicExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<BusinessLogicExceptionHandler> _logger;

        public BusinessLogicExceptionHandler(ILogger<BusinessLogicExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not UnprocessableException unprocessableException)
            {
                return false;
            }

            _logger.LogError(
                unprocessableException,
                "Exception occurred: {Message}",
                unprocessableException.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Business Violations",
                Detail = unprocessableException.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
