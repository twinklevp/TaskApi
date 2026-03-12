// Discriminated union — keeps endpoint handlers free of try/catch
namespace TaskApi.Models;

public abstract record TaskResult;

public record TaskSuccess(Task Task) : TaskResult;
public record TaskNotFound(Guid Id) : TaskResult;
public record TaskValidationError(string[] Errors) : TaskResult;