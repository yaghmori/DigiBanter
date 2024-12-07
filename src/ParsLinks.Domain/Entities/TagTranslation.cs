namespace ParsLinks.Domain.Entities;

public class TagTranslation: BaseEntity<int>
{
    public int TagId { get; set; }
    public int LanguageId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }

    // Navigation properties
    public Tag Tag { get; set; }
    public Language Language { get; set; }
}
