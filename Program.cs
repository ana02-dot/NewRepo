using UserManagementAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddLogging(); // Logging service for middleware
builder.Services.AddAuthentication(); // Add authentication services

var app = builder.Build();

// Configure the middleware pipeline

// 1. Error-handling middleware - this should run first to catch any unhandled exceptions
app.UseMiddleware<CustomExceptionMiddleware>();

// 2. Authentication middleware - validates tokens or user credentials
app.UseMiddleware<TokenValidationMiddleware>();

// 3. Logging middleware - logs HTTP requests and responses
app.UseMiddleware<RequestResponseLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();