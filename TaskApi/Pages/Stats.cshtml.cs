using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskApi.Data;
using TaskApi.Models;

namespace TaskApi.Pages;

public class StatsModel(ITaskRepository repo) : PageModel
{
    public TaskStats Stats { get; private set; } = new();

    public void OnGet() => Stats = repo.GetStats();
}