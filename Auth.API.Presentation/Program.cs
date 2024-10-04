using Auth.API.Core.Options;
using Auth.API.Infrastructure;
using Auth.API.Persistence;
using Auth.API.Presentation.Middleware;
using Auth.API.Presentation.Swagger;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt",
        rollingInterval: RollingInterval.Infinite,
        rollOnFileSizeLimit: true)
    .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();
builder.Services.Configure<MailConfig>(configuration.GetSection("MailConfig"));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowWebApp", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.ConfigureInfrastructure(configuration);
builder.Services.ConfigurePersistence(configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowWebApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
