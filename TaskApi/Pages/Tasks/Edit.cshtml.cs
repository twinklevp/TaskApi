using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskApi.Data;
using TaskApi.Models;

namespace TaskApi.Pages.Tasks;

public class EditModel(ITaskRepository repo) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public IActionResult OnGet(Guid id)
    {
        var task = repo.GetById(id);
        if (task is null) return NotFound();

        Input = new InputModel
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Status = task.Status,
            DueDate = task.DueDate.HasValue
                            ? DateOnly.FromDateTime(task.DueDate.Value.DateTime)
                            : null,
            Tags = string.Join(", ", task.Tags),
        };
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        var tags = (Input.Tags ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        repo.Update(Input.Id, task =>
        {
            task.Title = Input.Title;
            task.Description = Input.Description;
            task.Priority = Input.Priority;
            task.Status = Input.Status;
            task.DueDate = Input.DueDate.HasValue
                                 ? new DateTimeOffset(Input.DueDate.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero)
                                 : null;
        });

        return RedirectToPage("Index");
    }

    public class InputModel
    {
        public Guid Id { get; set; }
        [Required, MaxLength(200)] public string Title { get; set; } = "";
        [MaxLength(2000)] public string? Description { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public Models.TaskStatus Status { get; set; } = Models.TaskStatus.Todo;
        [DataType(DataType.Date)] public DateOnly? DueDate { get; set; }
        public string? Tags { get; set; }
    }
}