
using ParsLinks.Domain.Enums;

namespace ParsLinks.Shared.Dto.Response;
public class BaseBlogPostResponse
{
    public int Id { get; set; }
    public string Author { get; set; } = default!;
    public string? Image { get; set; }
    public string Title { get; set; } = default!;
    public string Summary { get; set; } = default!;
    public DateTime? PublishedAt { get; set; }
    public BlogPostStatusEnum Status { get; set; }
    public List<string> AvailableTranslations { get; set; } = new();
    public List<CategoryResponse> Categories { get; set; } = new();


}

