namespace DigiBanter.Domain.Entities;

public class Post:BaseEntity<int>
{
    public int AuthorId { get; set; }
    public int? CategoryId { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public int Status { get; set; } = 1; // Draft, Published, Archived
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User Author { get; set; }
    public Category Category { get; set; }
    public ICollection<PostTranslation> Translations { get; set; } = new List<PostTranslation>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
