using TaskApi.Models;

namespace TaskApi.Endpoints;

public record CreateTaskRequest(
    string Title,
    string? Description,
    TaskPriority? Priority,
    DateTimeOffset? DueDate,
    IReadOnlyList<string>? Tags
);

public record UpdateStatusRequest(Models.TaskStatus Status);