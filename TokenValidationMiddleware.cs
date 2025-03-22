namespace UserManagementAPI
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenValidationMiddleware> _logger;

        public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Extract the token from the Authorization header
                var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    _logger.LogWarning("Authorization header is missing or invalid.");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { error = "Unauthorized: Token is missing or invalid." });
                    return;
                }

                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                // Validate the token (you can integrate your own validation logic or use a library)
                if (!IsValidToken(token))
                {
                    _logger.LogWarning("Invalid token.");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { error = "Unauthorized: Invalid token." });
                    return;
                }

                // Token is valid, proceed to the next middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while validating the token.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "Internal server error." });
            }
        }

        private bool IsValidToken(string token)
        {
            // Placeholder for token validation logic (e.g., JWT validation)
            // Implement your own logic to validate the token, such as verifying its signature and claims
            return !string.IsNullOrEmpty(token) && token == "VALID_TOKEN"; // Example validation
        }

    }
}
