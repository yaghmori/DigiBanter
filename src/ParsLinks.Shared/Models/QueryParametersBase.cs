namespace ParsLinks.Shared.Models;

public class QueryParametersBase
{
    public string? Query { get; set; }
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 10;
    public bool Paged { get; set; } = true;
    public string? Lang { get; set; } = "en-US";

}
