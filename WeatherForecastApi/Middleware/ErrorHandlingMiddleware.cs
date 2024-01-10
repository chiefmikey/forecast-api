using System.Net;
using System.Text.Json;
using System.Net.Mime;

namespace WeatherForecastApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private const string UnhandledExceptionMessage = "An unhandled exception has occurred while executing the request.";

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            LogException(exception);

            var statusCode = MapExceptionToStatusCode(exception);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)statusCode;

            var response = CreateErrorResponse(statusCode, exception);

            var result = SerializeToJson(response);
            return context.Response.WriteAsync(result);
        }

        private void LogException(Exception exception)
        {
            _logger.LogError(exception, UnhandledExceptionMessage);
        }

        private HttpStatusCode MapExceptionToStatusCode(Exception exception)
        {
            return exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                // Add more cases here to handle other types of exceptions if necessary
                _ => HttpStatusCode.InternalServerError
            };
        }

        private ErrorResponse CreateErrorResponse(HttpStatusCode statusCode, Exception exception)
        {
            var response = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = exception.Message
            };

            if (_env.IsDevelopment())
            {
                response.StackTrace = exception.StackTrace;
            }

            return response;
        }

        private string SerializeToJson(ErrorResponse response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(response, options);
        }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
    }
}
