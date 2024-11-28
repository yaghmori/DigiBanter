namespace DigiBanter.Domain.Entities;

public class Language: BaseEntity<int>
{
    public string? Image { get; set; } 
    public string Code { get; set; } // e.g., "en", "es", "fr"
    public string Name { get; set; } // e.g., "English", "Spanish", "French"
    public bool IsDefault { get; set; } = false;

    // Navigation properties
    public ICollection<PostTranslation> PostTranslations { get; set; } = new List<PostTranslation>();
    public ICollection<CategoryTranslation> CategoryTranslations { get; set; } = new List<CategoryTranslation>();
    public ICollection<TagTranslation> TagTranslations { get; set; } = new List<TagTranslation>();
}
