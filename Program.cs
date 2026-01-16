using Microsoft.EntityFrameworkCore;
using PersonalTaskManager.Models;
using PersonalTaskManager.Repository;
using PersonalTaskManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultDbConnection"))
);

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Personal Task Manager API V1"
        );
        options.RoutePrefix = "";
    }
   );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
