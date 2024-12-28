using IODataLabs.OrderProcessingSystem.Application;
using IODataLabs.OrderProcessingSystem.Infrastructure.DataContext;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.InjectApplicationDependencies();
//builder.AddWebServices();

builder.Services.AddControllers();

// Register Swagger services
builder.Services.AddEndpointsApiExplorer(); // Required for generating Swagger API documentation
builder.Services.AddSwaggerGen(options =>
{
    // Optionally, you can customize the Swagger UI or API info
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "A sample API for Swagger demo",
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        options.RoutePrefix = string.Empty; // Optional: Set Swagger UI at the root
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
