namespace ParsLinks.Shared.Models;

public class QueryParametersBase
{
    public string? Query { get; set; }
    public int? Size { get; set; }
    public int? Index { get; set; }
    public bool? Paged { get; set; }
    public string? Lang { get; set; } = "en-US";

}
