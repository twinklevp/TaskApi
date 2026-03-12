
//  C# 14: Partial Member IMPLEMENTATION
//
//  This file provides the implementation for partial members
//  declared in TaskDbContext.cs.
//
//  Splitting the DbContext this way keeps:
//    - TaskDbContext.cs     → schema config + filter setup
//    - TaskDbContext.Hooks.cs → save-time business logic
//
//  If this file is deleted, the compiler silently removes
//  the OnTaskSaving() call — zero runtime cost.

namespace TaskApi.Data;

public sealed partial class TaskDbContext
{
    //Runs automatically before every SaveChanges() for Added/Modified tasks.
    partial void OnTaskSaving(Models.Task task)
    {
        task.Title = task.Title.Trim();

        // Null-condition assignment — set default description if missing
        task.Description ??= "No description provided";

        if (task.DueDate.HasValue && task.DueDate.Value < DateTimeOffset.UtcNow)
        {
            var isNew = (DateTimeOffset.UtcNow - task.CreatedAt).TotalSeconds < 5;
            if (isNew) task.DueDate = null;
        }
    }
}