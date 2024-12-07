using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EAllyfe.Api.Middlewares
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception : {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Detail = exception.Message,
                Title = exception.GetType().Name
            };

            switch (exception)
            {
                case DbUpdateException arg:
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Type = arg.GetType().Name,
                        Detail = "An unexpected error occurred. Please try again later.",
                        Title = "Unexpected error",
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
                case SqlException arg:
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Type = arg.GetType().Name,
                        Detail = "An unexpected error occurred. Please try again later.",
                        Title = "Unexpected error",
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
                case UnauthorizedAccessException arg:
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.Unauthorized,
                        Type = arg.GetType().Name,
                        Detail = "You are not authorized to access this resource.",
                        Title = arg.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;
                case KeyNotFoundException arg:
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.NotFound,
                        Type = arg.GetType().Name,
                        Title = "Key not found.",
                        Detail = arg.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                case OperationCanceledException arg:
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.RequestTimeout,
                        Type = arg.GetType().Name,
                        Title = "The request was canceled.",
                        Detail = arg.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                    break;
                case BadHttpRequestException arg:
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Type = arg.GetType().Name,
                        Title = "The request was invalid",
                        Detail = arg.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                case ArgumentNullException arg:
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.NotFound,
                        Type = arg.GetType().Name,
                        Title = "An unexpected error occurred",
                        Detail = arg.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                    break;

                default:
                    problemDetails = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Type = exception.GetType().Name,
                        Title = "An unexpected error occurred. Please try again later.",
                        Detail = exception.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                    };
                    break;

            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }

}
