// ============================================================
//  Filters/Endpoints/ValidationFilter.cs
//  ✅ Endpoint Filter — runs before/after specific endpoints
//
//  Unlike middleware (which is global), endpoint filters
//  are attached to individual routes or groups.
//  This filter validates that POST/PATCH request bodies
//  are not null before the handler runs.
// ============================================================

namespace TaskApi.Filters.Endpoints;

public class ValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        // ─── Before the handler runs ──────────────────────────
        // Check all arguments — if any bound model is null, reject early
        foreach (var arg in context.Arguments)
        {
            if (arg is null)
                return Results.BadRequest(new
                {
                    error = "Request body is required.",
                    success = false
                });
        }

        // ─── Run the actual endpoint handler ──────────────────
        var result = await next(context);

        // ─── After the handler runs ───────────────────────────
        // You can inspect or transform the result here if needed
        return result;
    }
}