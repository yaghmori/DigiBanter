namespace DigiBanter.Domain.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
