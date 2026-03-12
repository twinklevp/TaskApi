// ============================================================
//  Models/TaskExtensions.cs
//  ✅ C# 14: Extension properties via static extension methods
//  (extension block syntax requires latest C# 14 preview SDK)
// ============================================================

namespace TaskApi.Models;

public static class TaskExtensions
{
    // ✅ Extension property equivalent — IsOverdue
    public static bool IsOverdue(this Task task) =>
        task.DueDate.HasValue
        && task.DueDate.Value < DateTimeOffset.UtcNow
        && task.Status is not (TaskStatus.Done or TaskStatus.Cancelled);

    // ✅ Extension property equivalent — DisplayTitle
    public static string DisplayTitle(this Task task) =>
        $"[{task.Priority}] {task.Title}";

    // ✅ Extension property equivalent — DueDateLabel
    public static string DueDateLabel(this Task task) =>
        task.DueDate.HasValue
            ? task.DueDate.Value.ToString("dd MMM yyyy")
            : "No due date";

    // ✅ C# 14: params IEnumerable<T> — still valid here
    public static bool HasAnyTag(this Task task, params IEnumerable<string> tags) =>
        task.Tags.Any(t => tags.Contains(t, StringComparer.OrdinalIgnoreCase));
}