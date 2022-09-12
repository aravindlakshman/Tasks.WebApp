using System.Text.Json.Serialization;
using Tasks.WebAPI.Datalayer;
using Microsoft.EntityFrameworkCore;
using Tasks.WebAPI.Interfaces;
using Tasks.WebAPI.Services;
using Microsoft.OpenApi.Models;
using Tasks.Domain.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options=>options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowAnyOrigin", options => options.AllowAnyOrigin());
    c.AddPolicy("AllowAnyHeader",options=>options.AllowAnyHeader());
    c.AddPolicy("AllowAnyMethod",options=>options.AllowAnyMethod());
});

var connectionString = builder.Configuration.GetConnectionString("connectionString");
builder.Services.AddDbContext<TasksDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasks.WebApp", Version = "v1" });
    
});

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();
    


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
