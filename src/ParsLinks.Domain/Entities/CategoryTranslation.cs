namespace ParsLinks.Domain.Entities;

public class CategoryTranslation : BaseEntity<int>
{
    public int CategoryId { get; set; }
    public int LanguageId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }

    // Navigation properties
    public Category Category { get; set; }
    public Language Language { get; set; }
}
