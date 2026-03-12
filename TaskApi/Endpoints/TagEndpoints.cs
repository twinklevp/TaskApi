// C# 14: params IEnumerable<T> via HasAnyTag extension
using TaskApi.Data;
using TaskApi.Filters;
using TaskApi.Models;

namespace TaskApi.Endpoints;

public static class TagEndpoints
{
    public static IEndpointRouteBuilder MapTagEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tasks/tags/match", MatchByTags)
           .WithTags("Tags")
           .WithOpenApi()
           .WithSummary("Find tasks matching any of the given tags")
           .WithDescription("Pass comma-separated tags e.g. ?tags=bug,api,devops")
           .Produces<List<Models.Task>>(200);

        return app;
    }

    private static IResult MatchByTags(string tags, ITaskRepository repo)
    {
        var tagList = tags.Split(',',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var matched = repo
            .GetAll(new TaskFilter())
            .Where(t => t.HasAnyTag(tagList)) // C# 14: params IEnumerable<string>
            .ToList();

        return Results.Ok(matched);
    }
}