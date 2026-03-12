using TaskApi.Models;

namespace TaskApi.Filters;

public record TaskFilter(
    Models.TaskStatus? Status = null,
    TaskPriority? Priority = null,
    string? Tag = null,
    bool? OverdueOnly = null
);