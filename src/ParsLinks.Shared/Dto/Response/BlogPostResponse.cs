
namespace ParsLinks.Shared.Dto.Response;
public class BlogPostResponse : BaseBlogPostResponse
{
    public string Content { get; set; } = default!;
    public string? Category { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Language { get; set; } = default!;
    public int EstimatedReadingTime { get; set; } // In minutes


}

