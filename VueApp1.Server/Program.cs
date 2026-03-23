using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Supabase;
using VueApp1.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add OpenAPI (Swagger)
builder.Services.AddOpenApi();

// Read configuration from appsettings.json and environment-specific files
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "*" };
var allowSwagger = builder.Configuration.GetValue<bool>("AllowSwagger", true);

var supaBaseUrl = builder.Environment.IsDevelopment()? builder.Configuration["SupabaseUrlDev"] : builder.Configuration["SupabaseUrlProd"];
var supaBaseKey = builder.Environment.IsDevelopment()? builder.Configuration["SupabaseKeyDev"] : builder.Configuration["SupabaseKeyProd"];

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(supaBaseUrl, supaBaseKey, new SupabaseOptions { AutoConnectRealtime = true })
);

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
