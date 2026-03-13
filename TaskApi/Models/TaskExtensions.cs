//  Models/TaskExtensions.cs
//  C# 14: Extension properties via static extension methods

namespace TaskApi.Models;

public static class TaskExtensions
{
    // Extension property 
    public static bool IsOverdue(this Task task) =>
        task.DueDate.HasValue
        && task.DueDate.Value < DateTimeOffset.UtcNow
        && task.Status is not (TaskStatus.Done or TaskStatus.Cancelled);

    public static bool HasAnyTag(this Task task, params IEnumerable<string> tags) =>
        task.Tags.Any(t => tags.Contains(t, StringComparer.OrdinalIgnoreCase));
}