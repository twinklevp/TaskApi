// ============================================================
//  Filters/Endpoints/LoggingFilter.cs
//  ✅ Endpoint Filter — request/response logging per route group
//
//  Attached only to the /api/tasks group — not globally.
//  Logs method, path, and response status for every request
//  that hits the tasks endpoints.
// ============================================================

namespace TaskApi.Filters.Endpoints;

public class LoggingFilter(ILogger<LoggingFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.HttpContext.Request;

        // ─── Before ───────────────────────────────────────────
        logger.LogInformation(
            "[TaskApi] → {Method} {Path}",
            request.Method,
            request.Path);

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // ─── Handler ──────────────────────────────────────────
        var result = await next(context);

        // ─── After ────────────────────────────────────────────
        stopwatch.Stop();

        logger.LogInformation(
            "[TaskApi] ← {Method} {Path} ({Elapsed}ms)",
            request.Method,
            request.Path,
            stopwatch.ElapsedMilliseconds);

        return result;
    }
}