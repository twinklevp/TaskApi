//  Endpoints/TaskEndpoints.cs — CRUD routes for /api/tasks
//  Endpoint Filters: ValidationFilter + LoggingFilter
//  Named Query Filter: IgnoreQueryFilters("ActiveOnly")

using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Filters;
using TaskApi.Filters.Endpoints;
using TaskApi.Models;

namespace TaskApi.Endpoints;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks")
                       .WithTags("Tasks")
                       .WithOpenApi()
                       // Endpoint Filter: LoggingFilter on the whole group
                       //    Logs every request/response for /api/tasks/* endpoints
                       //    Before endpoint filters existed, you'd need global middleware
                       //    or manual logging in every handler
                       .AddEndpointFilter<LoggingFilter>();

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapGet("/all", GetAllIncludingCancelled);

        //    Endpoint Filter: ValidationFilter only on mutating endpoints
        //    POST and PATCH get null-body validation — GET and DELETE don't need it
        group.MapPost("/", Create)
             .AddEndpointFilter<ValidationFilter>();

        group.MapPatch("/{id:guid}/status", UpdateStatus)
             .AddEndpointFilter<ValidationFilter>();

        group.MapDelete("/{id:guid}", Delete);

        return app;
    }

    // Handlers

    private static IResult GetAll(
        ITaskRepository repo,
        Models.TaskStatus? status = null,
        TaskPriority? priority = null,
        string? tag = null,
        bool? overdueOnly = null) =>
        Results.Ok(repo.GetAll(new TaskFilter(status, priority, tag, overdueOnly)).ToList());

    //    Named Query Filter: this endpoint includes Cancelled tasks
    //    by calling IgnoreQueryFilters("ActiveOnly")
    //    Before EF Core 10: IgnoreQueryFilters() disabled ALL filters — too broad
    //    Now: only the "ActiveOnly" filter is disabled, others stay active
    private static IResult GetAllIncludingCancelled(TaskDbContext db) =>
        Results.Ok(
            db.Tasks
              // .IgnoreQueryFilters("ActiveOnly")
              .OrderByDescending(t => t.CreatedAt)
              .ToList());

    private static IResult GetById(Guid id, ITaskRepository repo) =>
        repo.GetById(id) is { } task
            ? Results.Ok(task)
            : Results.NotFound(new { error = $"Task {id} not found" });

    private static IResult Create(CreateTaskRequest req, ITaskRepository repo)
    {
        var task = new Models.Task
        {
            Id = Guid.NewGuid(),
            Title = req.Title,
            Description = req.Description,
            Priority = req.Priority ?? TaskPriority.Medium,
            DueDate = req.DueDate,
            Tags = req.Tags ?? [],
        };
        repo.Add(task);
        return Results.Created($"/api/tasks/{task.Id}", task);
    }

    private static IResult UpdateStatus(Guid id, UpdateStatusRequest req, ITaskRepository repo)
    {
        var task = repo.Update(id, t => t.Status = req.Status);
        return task is null
            ? Results.NotFound(new { error = $"Task {id} not found" })
            : Results.Ok(task);
    }

    private static IResult Delete(Guid id, ITaskRepository repo) =>
        repo.Delete(id) ? Results.NoContent() : Results.NotFound();
}