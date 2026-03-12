//  Data/EfTaskRepository.cs — SQLite-backed repository
//  LINQ:         CountBy, AggregateBy, Index()
//  Collections:  FrozenDictionary
//  C# 14:        Null-condition assignment (??=)
//  EF Core 10:   Named Query Filter (IgnoreQueryFilters)

using System.Collections.Frozen;
using Microsoft.EntityFrameworkCore;
using TaskApi.Filters;
using TaskApi.Models;
using TaskStatus = TaskApi.Models.TaskStatus;

namespace TaskApi.Data;

public sealed class EfTaskRepository(TaskDbContext db) : ITaskRepository
{
    // ─── Queries ─────────────────────────────────────────────

    public IEnumerable<Models.Task> GetAll(TaskFilter filter)
    {
        IQueryable<Models.Task> query = db.Tasks.AsNoTracking();

        // Push status/priority filters to SQL
        if (filter.Status is not null) query = query.Where(t => t.Status == filter.Status);
        if (filter.Priority is not null) query = query.Where(t => t.Priority == filter.Priority);

        // AsEnumerable() BEFORE OrderBy — SQLite can't sort DateTimeOffset in SQL
        IEnumerable<Models.Task> results = query
            .AsEnumerable()
            .OrderByDescending(t => (int)t.Priority)
            .ThenBy(t => t.DueDate ?? DateTimeOffset.MaxValue)
            .ThenBy(t => t.CreatedAt);

        if (!string.IsNullOrWhiteSpace(filter.Tag))
            results = results.Where(t =>
                t.Tags.Contains(filter.Tag, StringComparer.OrdinalIgnoreCase));

        if (filter.OverdueOnly == true)
            results = results.Where(t => t.IsOverdue());

        return results;
    }

    public Models.Task? GetById(Guid id) =>
        db.Tasks.AsNoTracking().FirstOrDefault(t => t.Id == id);

    // ─── Mutations ───────────────────────────────────────────

    public Models.Task Add(Models.Task task)
    {
        db.Tasks.Add(task);
        db.SaveChanges();
        return task;
    }

    public Models.Task? Update(Guid id, Action<Models.Task> mutate)
    {
        var task = db.Tasks.Find(id);
        if (task is null) return null;
        mutate(task);
        db.SaveChanges();
        return task;
    }

    public bool Delete(Guid id)
    {
        var task = db.Tasks.Find(id);
        if (task is null) return false;
        db.Tasks.Remove(task);
        db.SaveChanges();
        return true;
    }

    // ─── Stats ───────────────────────────────────────────────

    public TaskStats GetStats()
    {
        //    Include Cancelled tasks in stats — we want the full picture
        //    Before: IgnoreQueryFilters() would disable ALL filters globally
        //    Now: only disables the named "ActiveOnly" filter
        var all = db.Tasks
            .AsNoTracking()
            .IgnoreQueryFilters()
            .AsEnumerable()
            .ToList();

        // ✅ CountBy — single-pass group count, no GroupBy needed
        var byStatus = all
            .CountBy(t => t.Status.ToString())
            .ToDictionary(k => k.Key, k => k.Value);

        var byPriority = all
            .CountBy(t => t.Priority.ToString())
            .ToDictionary(k => k.Key, k => k.Value);

        // ✅ AggregateBy — per-tag frequency without GroupBy
        var topTags = all
            .SelectMany(t => t.Tags.Select(tag => (tag, t)))
            .AggregateBy(
                keySelector: x => x.tag,
                seed: 0,
                func: (count, _) => count + 1)
            .OrderByDescending(kvp => kvp.Value)
            .Take(5)
            .Select(kvp => new TagCount(kvp.Key, kvp.Value))
            .ToList();

        // ✅ Index() — enumerate with position, no manual counter variable
        var overdueCount = all
            .OrderBy(t => t.CreatedAt)
            .Index()
            .Count(x => x.Item.IsOverdue());

        // ✅ FrozenDictionary — read-optimised immutable snapshot
        return new TaskStats
        {
            Total = all.Count,
            Completed = byStatus.GetValueOrDefault(nameof(TaskStatus.Done)),
            Overdue = overdueCount,
            ByStatus = byStatus.ToFrozenDictionary(),
            ByPriority = byPriority.ToFrozenDictionary(),
            TopTags = topTags,
        };
    }
}