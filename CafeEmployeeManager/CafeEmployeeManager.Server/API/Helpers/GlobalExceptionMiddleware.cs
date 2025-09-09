using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace CafeEmployeeManager.Server.API.Helpers
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception");
                if (IsDeadlockException(ex))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(new { error = "A database deadlock occurred. Please retry your request." })
                    );
                }
                else
                {
                    context.Response.StatusCode = ex is ArgumentException ? 400 : 500;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(new { error = ex.Message })
                    );
                }
            }
        }

        private static bool IsDeadlockException(Exception ex)
        {
            if (ex is SqlException sqlEx && sqlEx.Number == 1205)
                return true;
            if (ex.Message.Contains("deadlock", StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred. Please try again later.";

            if (IsDeadlockException(exception))
            {
                code = HttpStatusCode.Conflict;
                message = "A database deadlock occurred. Please retry your request.";
            }
            else if (exception is TimeoutException || exception.Message.Contains("Connection Timeout Expired"))
            {
                code = HttpStatusCode.RequestTimeout;
                message = "Database connection timed out. Please check your network or try again later.";
            }
            else if (exception.Message.Contains("pre-login handshake"))
            {
                code = HttpStatusCode.ServiceUnavailable;
                message = "Unable to connect to the database server. Please try again in a few minutes.";
            }
            else if (exception.Message.Contains("network-related"))
            {
                code = HttpStatusCode.BadGateway;
                message = "Database server is not reachable. Contact support if the issue persists.";
            }

            var result = JsonSerializer.Serialize(new
            {
                error = message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
