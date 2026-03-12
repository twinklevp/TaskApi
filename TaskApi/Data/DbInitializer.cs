using Microsoft.EntityFrameworkCore;
using TaskApi.Models;
using TaskStatus = TaskApi.Models.TaskStatus;

namespace TaskApi.Data;

public static class DbInitializer
{
    public static void Initialize(TaskDbContext db)
    {
        db.Database.Migrate();
        if (db.Tasks.Any()) return;

        db.Tasks.AddRange(
            Make("Set up CI/CD pipeline", TaskPriority.High, TaskStatus.InProgress, 3, "devops", "infrastructure"),
            Make("Write API documentation", TaskPriority.Medium, TaskStatus.Todo, 7, "docs", "api"),
            Make("Fix login regression", TaskPriority.Critical, TaskStatus.Blocked, -1, "bug", "auth"),
            Make("Upgrade to .NET 10", TaskPriority.High, TaskStatus.Done, -5, "devops", "dotnet"),
            Make("Add dark mode", TaskPriority.Low, TaskStatus.Todo, 14, "ui", "feature"),
            Make("Performance profiling", TaskPriority.Medium, TaskStatus.Todo, -2, "performance", "dotnet")
        );
        db.SaveChanges();
    }

    private static Models.Task Make(
        string title, TaskPriority priority, TaskStatus status,
        int dueDaysFromNow, params string[] tags)
    {
        var task = new Models.Task
        {
            Id = Guid.NewGuid(),
            Title = title,
            Priority = priority,
            Status = status,
            DueDate = DateTimeOffset.UtcNow.AddDays(dueDaysFromNow),
            Tags = tags,
        };

        // C# 14: Null-condition assignment
        task.Description ??= "No description provided";

        return task;
    }
}