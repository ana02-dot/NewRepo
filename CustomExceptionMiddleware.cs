namespace UserManagementAPI
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass the request to the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An unhandled exception occurred during request processing.");

                // Return the error response in JSON format
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // Build a structured JSON response
            var errorResponse = new
            {
                error = "Internal server error.",
                details = exception.Message // Optional: Include exception details for debugging
            };

            return context.Response.WriteAsJsonAsync(errorResponse);
        }

    }
}
