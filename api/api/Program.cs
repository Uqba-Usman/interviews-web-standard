using api;
using api.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; // Optional: for better readability
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:3000", "http://localhost:3001") // Add allowed origins
                        .AllowAnyMethod()                  // Allows any HTTP method (GET, POST, etc.)
                        .AllowAnyHeader()                  // Allows any headers
                        .AllowCredentials());              // Allows credentials (if required)
});

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error != null)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred!",
                Detail = exceptionHandlerPathFeature.Error.Message
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    });
});

// Custom exception handling middleware
//app.UseMiddleware<CustomExceptionHandlerMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin"); // Enable CORS

app.UseAuthorization();

app.MapControllers();

app.Run();
