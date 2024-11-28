namespace DigiBanter.Domain.Entities;

public class TagTranslation: BaseEntity<int>
{
    public int TagTranslationId { get; set; }
    public int TagId { get; set; }
    public int LanguageId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }

    // Navigation properties
    public Tag Tag { get; set; }
    public Language Language { get; set; }
}
