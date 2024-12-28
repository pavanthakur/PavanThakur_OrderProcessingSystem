using System.Text;

namespace IODataLabs.OrderProcessingSystem.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate requestDelegate, ILogger<LoggingMiddleware> logger)
        {
            _next = requestDelegate;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var requestBody = await GetRequestBodyAsync(request);
            _logger.LogInformation(@"Request: {Method} {Url} {QueryString} Body: {RequestBody}",
                request.Method,
                request.Path,
                request.QueryString,
                requestBody);
            var originalBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await _next(context);

                context.Response.Body = originalBodyStream;
                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                _logger.LogInformation("Response: {StatusCode} Body: {ResponseBody}",
                    context.Response.StatusCode,
                    responseBody);

                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
        private async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            if (request.ContentLength > 0 && request.Body.CanSeek)
            {
                request.EnableBuffering(); // Allows reading the request body multiple times
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    request.Body.Seek(0, SeekOrigin.Begin); // Reset the body stream position
                    return body;
                }
            }
            return string.Empty;
        }
    }
}
