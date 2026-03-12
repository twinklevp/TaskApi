using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using TaskApi.Endpoints;
using TaskApi.Models;

namespace TaskApi.Infrastructure;

[JsonSerializable(typeof(Models.Task))]
[JsonSerializable(typeof(List<Models.Task>))]
[JsonSerializable(typeof(TaskStats))]
[JsonSerializable(typeof(TagCount))]
[JsonSerializable(typeof(CreateTaskRequest))]
[JsonSerializable(typeof(UpdateStatusRequest))]
[JsonSerializable(typeof(ValidationProblemDetails))]
internal partial class TaskApiJsonContext : JsonSerializerContext { }