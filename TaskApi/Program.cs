using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Endpoints;
using TaskApi.Filters.Endpoints;
using TaskApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Database
var dbPath = builder.Configuration.GetValue<string>("Database:Path") ?? "tasks.db";

builder.Services.AddDbContext<TaskDbContext>(opt =>
    opt.UseSqlite($"Data Source={dbPath}")
       .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
       .EnableDetailedErrors(builder.Environment.IsDevelopment()));

builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();

// Endpoint Filters
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddScoped<LoggingFilter>();

builder.Services.AddRazorPages();

// OpenAPI
builder.Services.AddOpenApi(OpenApiConfig.Configure);

var app = builder.Build();

// Database initialisation
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    DbInitializer.Initialize(db);
}

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.MapRazorPages();
app.MapGet("/", () => Results.Redirect("/Tasks"));
app.MapTaskEndpoints();
app.MapStatsEndpoints();
app.MapTagEndpoints();

app.Run();