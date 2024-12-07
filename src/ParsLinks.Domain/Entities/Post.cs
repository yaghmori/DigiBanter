namespace ParsLinks.Domain.Entities;

public class Post:BaseEntity<int>
{
    public Guid? AuthorId { get; set; }
    public int? CategoryId { get; set; }
    public string? Image { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public int Status { get; set; } = 1; // Draft, Published, Archived

    // Navigation properties
    public User? Author { get; set; }
    public Category? Category { get; set; }
    public ICollection<PostTranslation> Translations { get; set; } = new List<PostTranslation>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
