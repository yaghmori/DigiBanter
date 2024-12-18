namespace ParsLinks.Shared.Models;

public class BlogPostQueryParameters : QueryParametersBase
{
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string? Status { get; set; } = "all";


}
