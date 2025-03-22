namespace UserManagementAPI
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log HTTP method and request path
            var method = context.Request.Method;
            var path = context.Request.Path;
            _logger.LogInformation($"HTTP Request: {method} {path}");

            // Invoke the next middleware in the pipeline
            await _next(context);

            // Log the response status code
            var statusCode = context.Response.StatusCode;
            _logger.LogInformation($"HTTP Response: Status Code {statusCode}");
        }

    }
}
