using TaskApi.Data;
using TaskApi.Models;

namespace TaskApi.Endpoints;

public static class StatsEndpoints
{
    public static IEndpointRouteBuilder MapStatsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tasks/stats", (ITaskRepository repo) => Results.Ok(repo.GetStats()))
           .WithTags("Stats")
           .WithOpenApi()
           .WithSummary("Aggregate task statistics")
           .WithDescription("Uses CountBy, AggregateBy, Index(), FrozenDictionary")
           .Produces<TaskStats>(200);

        return app;
    }
}