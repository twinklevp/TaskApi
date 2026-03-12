// ✅ .NET 10: built-in OpenAPI 3.1 — no Swashbuckle needed
using Microsoft.AspNetCore.OpenApi;

namespace TaskApi.Infrastructure;

public static class OpenApiConfig
{
    public static void Configure(OpenApiOptions options)
    {
        options.AddDocumentTransformer(async (doc, _, _) =>
        {
            doc.Info.Title = "Task API — .NET 10 POC";
            doc.Info.Version = "v1";
            doc.Info.Description = "Showcases Minimal APIs, OpenAPI 3.1, C# 14, LINQ improvements";
            await Task.CompletedTask;
        });
    }
}