using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskApi.Data;
using TaskApi.Filters;
using TaskApi.Models;

namespace TaskApi.Pages.Tasks;

public class IndexModel(ITaskRepository repo) : PageModel
{
    public IEnumerable<Models.Task> Tasks { get; private set; } = [];
    public TaskFilter Filter { get; private set; } = new();

    public void OnGet(
        Models.TaskStatus? status = null,
        TaskPriority? priority = null,
        string? tag = null,
        bool? overdueOnly = null)
    {
        Filter = new TaskFilter(status, priority, tag, overdueOnly);
        Tasks = repo.GetAll(Filter);
    }

    public IActionResult OnPostDelete(Guid id)
    {
        repo.Delete(id);
        return RedirectToPage();
    }
}