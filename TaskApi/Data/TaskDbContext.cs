//  Data/TaskDbContext.cs — EF Core DbContext
//  C# 14:     Partial Members
//  EF Core 10: Named Query Filters

using Microsoft.EntityFrameworkCore;
using TaskApi.Models;

namespace TaskApi.Data;

//    C# 14: partial class — split across two files
//    TaskDbContext.cs       → entity config + named query filters
//    TaskDbContext.Hooks.cs → save hook implementations
public sealed partial class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
{
    public DbSet<Models.Task> Tasks => Set<Models.Task>();

    //    C# 14: Partial member DECLARATION
    //    Signature lives here — implementation is in TaskDbContext.Hooks.cs
    //    If no implementation is provided, the compiler removes the call entirely (zero cost)
    partial void OnTaskSaving(Models.Task task);

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Models.Task>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).HasMaxLength(2000);

            // Store enums as readable strings
            entity.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
            entity.Property(t => t.Priority).HasConversion<string>().HasMaxLength(20);

            // Store tags as CSV
            entity.Property(t => t.Tags)
                  .HasConversion(
                      tags => string.Join(',', tags),
                      raw => raw.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                  .HasMaxLength(500);

            entity.Property(t => t.CreatedAt).IsRequired();

            entity.HasIndex(t => t.Status);
            entity.HasIndex(t => t.Priority);

            //    .net 10: Named Query Filter
            //    Filters out Cancelled tasks from ALL queries by default.
            //    Before EF Core 10: HasQueryFilter() was unnamed — you could only
            //    disable ALL filters with IgnoreQueryFilters(), not individual ones.
            //    Now: IgnoreQueryFilters("ActiveOnly") disables just this one filter.
            entity.HasQueryFilter("ActiveOnly", t => t.Status != Models.TaskStatus.Cancelled);
        });
    }

    public override int SaveChanges()
    {
        RunSaveHooks();
        return base.SaveChanges();
    }

    private void RunSaveHooks()
    {
        foreach (var entry in ChangeTracker.Entries<Models.Task>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            // C# 14: calls the partial method — implementation in Hooks.cs
            OnTaskSaving(entry.Entity);
        }
    }
}