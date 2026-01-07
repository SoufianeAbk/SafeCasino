using System.Diagnostics;

namespace SafeCasino.Web.Middleware
{
    /// <summary>
    /// Middleware voor request logging en performance monitoring
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = context.TraceIdentifier;
            var stopwatch = Stopwatch.StartNew();

            // Log incoming request
            _logger.LogInformation(
                "📥 Incoming Request | ID: {RequestId} | Method: {Method} | Path: {Path} | IP: {IP}",
                requestId,
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress);

            // Store original response stream
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);

                    stopwatch.Stop();

                    // Log outgoing response
                    _logger.LogInformation(
                        "📤 Outgoing Response | ID: {RequestId} | Status: {StatusCode} | Duration: {DurationMs}ms",
                        requestId,
                        context.Response.StatusCode,
                        stopwatch.ElapsedMilliseconds);

                    // Copy response body to original stream
                    await responseBody.CopyToAsync(originalBodyStream);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();

                    _logger.LogError(
                        ex,
                        "❌ Exception in Request | ID: {RequestId} | Duration: {DurationMs}ms",
                        requestId,
                        stopwatch.ElapsedMilliseconds);

                    throw;
                }
                finally
                {
                    context.Response.Body = originalBodyStream;
                }
            }
        }
    }

    /// <summary>
    /// Extension method om middleware toe te voegen
    /// </summary>
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}