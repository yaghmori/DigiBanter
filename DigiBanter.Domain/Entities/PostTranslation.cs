namespace DigiBanter.Domain.Entities;

public class PostTranslation:BaseEntity<int>
{
    public int PostId { get; set; }
    public int LanguageId { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Content { get; set; }

    // Navigation properties
    public Post Post { get; set; }
    public Language Language { get; set; }
}
