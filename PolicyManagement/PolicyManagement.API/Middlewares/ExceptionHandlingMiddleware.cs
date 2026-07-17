using FluentValidation;
using PolicyManagement.Core.Entities.Common;
using PolicyManagement.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace PolicyManagement.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var statusCode = exception switch
            {
                ValidationException =>
                    HttpStatusCode.BadRequest,

                BadRequestException =>
                    HttpStatusCode.BadRequest,

                NotFoundException =>
                    HttpStatusCode.NotFound,

                ConflictException =>
                    HttpStatusCode.Conflict,

                UnauthorizedAccessException =>
                    HttpStatusCode.Unauthorized,

                _ =>
                    HttpStatusCode.InternalServerError
            };

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(
                    exception,
                    "An unexpected error occurred. TraceId: {TraceId}",
                    context.TraceIdentifier);
            }
            else
            {
                _logger.LogWarning(
                    exception,
                    "Request failed with status code {StatusCode}. TraceId: {TraceId}",
                    (int)statusCode,
                    context.TraceIdentifier);
            }

            var response = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = exception is ValidationException
             ? "One or more validation errors occurred."
             : statusCode == HttpStatusCode.InternalServerError
                 ? "An unexpected error occurred."
                 : exception.Message,
                    TraceId = context.TraceIdentifier,
                    Errors = exception is ValidationException validationException
             ? validationException.Errors
                 .GroupBy(error => error.PropertyName)
                 .ToDictionary(
                     group => group.Key,
                     group => group
                         .Select(error => error.ErrorMessage)
                         .Distinct()
                         .ToArray())
             : null
            };

            context.Response.StatusCode = response.StatusCode;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            await context.Response.WriteAsync(json);
        }
    }
}
