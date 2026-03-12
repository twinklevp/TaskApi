using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskApi.Data;
using TaskApi.Models;

namespace TaskApi.Pages.Tasks;

public class CreateModel(ITaskRepository repo) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        var tags = (Input.Tags ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        repo.Add(new Models.Task
        {
            Id = Guid.NewGuid(),
            Title = Input.Title,
            Description = Input.Description,
            Priority = Input.Priority,
            Status = Input.Status,
            DueDate = Input.DueDate.HasValue
    ? new DateTimeOffset(Input.DueDate.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero)
    : null,
            Tags = tags,
        });

        return RedirectToPage("Index");
    }

    public class InputModel
    {
        [Required, MaxLength(200)] public string Title { get; set; } = "";
        [MaxLength(2000)] public string? Description { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public Models.TaskStatus Status { get; set; } = Models.TaskStatus.Todo;
        [DataType(DataType.Date)] public DateOnly? DueDate { get; set; }
        public string? Tags { get; set; }
    }
}