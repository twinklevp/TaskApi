using TaskApi.Filters;
using TaskApi.Models;

namespace TaskApi.Data;

public interface ITaskRepository
{
    IEnumerable<Models.Task> GetAll(TaskFilter filter);
    Models.Task? GetById(Guid id);
    Models.Task Add(Models.Task task);
    Models.Task? Update(Guid id, Action<Models.Task> mutate);
    bool Delete(Guid id);
    TaskStats GetStats();
}