namespace TaskApi.Models;

public class TaskStats
{
    public int Total { get; set; }
    public int Completed { get; set; }
    public int Overdue { get; set; }

    public double CompletionRate =>
        Total == 0 ? 0 : Math.Round((double)Completed / Total * 100, 2);

    public IReadOnlyDictionary<string, int> ByPriority { get; init; } = new Dictionary<string, int>();
    public IReadOnlyDictionary<string, int> ByStatus { get; init; } = new Dictionary<string, int>();
    public IReadOnlyList<TagCount> TopTags { get; init; } = [];
}

public record TagCount(string Tag, int Count);