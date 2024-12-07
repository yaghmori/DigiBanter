namespace ParsLinks.Domain.Entities;

public class PostTranslation:BaseEntity<int>
{
    public int PostId { get; set; }
    public int LanguageId { get; set; }
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Content { get; set; } = default!;

    // Navigation properties
    public Post Post { get; set; }
    public Language Language { get; set; }
}
