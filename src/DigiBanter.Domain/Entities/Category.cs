namespace DigiBanter.Domain.Entities;

public class Category:BaseEntity<int>
{

    // Navigation properties
    public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
