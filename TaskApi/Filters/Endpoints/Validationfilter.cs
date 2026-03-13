//  Filters/Endpoints/ValidationFilter.cs
//  Endpoint Filter — runs before/after specific endpoints
//  Unlike middleware (which is global), endpoint filters
//  are attached to individual routes or groups.
//  This filter validates that POST/PATCH request bodies
//  are not null before the handler runs.

namespace TaskApi.Filters.Endpoints;

public class ValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
    EndpointFilterInvocationContext context,
    EndpointFilterDelegate next)
    {
        // This will hit the debugger
        foreach (var arg in context.Arguments)
        {
            if (arg is null)
                return Results.BadRequest(new { error = "Request body is required." });
        }

        var result = await next(context);
        return result;
    }
}