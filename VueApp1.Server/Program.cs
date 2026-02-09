using Microsoft.EntityFrameworkCore;
using VueApp1.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add OpenAPI (Swagger)
builder.Services.AddOpenApi();

// Read configuration from appsettings.json and environment-specific files
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "*" };
var allowSwagger = builder.Configuration.GetValue<bool>("AllowSwagger", true);

// Determine connection string key based on environment
string connectionStringKey = builder.Environment.IsProduction() ? "prod_DefaultConnection" : "dev_DefaultConnection";

// Validate that connection string is available (from Azure App Service or appsettings)
var connectionString = builder.Configuration.GetConnectionString(connectionStringKey);

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException(
        $"Connection string '{connectionStringKey}' not found. " +
        $"Environment: {builder.Environment.EnvironmentName}. " +
        $"In Azure App Service, ensure you have set a connection string named '{connectionStringKey}' under Settings → Configuration → Connection strings."
    );
}

// Add CORS policy with environment-specific origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (allowedOrigins.Contains("*"))
        {
            policy.AllowAnyOrigin() // Dev environment
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(allowedOrigins) // Production: specific origins
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

builder.Services.AddScoped<DapperService>();
builder.Services.AddHttpClient<AddressService>();
builder.Services.AddScoped<SuggestionService>();

var app = builder.Build();

// Serve static files
app.UseDefaultFiles();
app.MapStaticAssets();

// Enable CORS BEFORE routing
app.UseCors("AllowFrontend");

// Enable HTTPS in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// Swagger in development only
if (app.Environment.IsDevelopment() || allowSwagger)
{
    app.MapOpenApi();
}

// Fallback to index.html for SPA
app.MapFallbackToFile("/index.html");

app.Run();
