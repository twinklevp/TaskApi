// ✅ C# 14: field keyword for validated auto-properties
namespace TaskApi.Models;

public class Task
{
    public required Guid Id { get; init; }
    public required string Title
    {
        // ✅ C# 14: 'field' refers to the compiler-generated backing field
        get;
        set => field = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Title cannot be blank")
            : value.Trim();
    }

    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DueDate { get; set; }

    // ✅ C# 14: collection expression initializer
    // Models/Task.cs
    public IReadOnlyList<string> Tags { get; set; } = [];
}
