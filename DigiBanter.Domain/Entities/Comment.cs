namespace DigiBanter.Domain.Entities;

public class Comment:BaseEntity<int>
{
    public int PostId { get; set; }
    public Guid? UserId { get; set; }
    public string Content { get; set; }
    public int? ParentId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Post Post { get; set; }
    public User User { get; set; }
    public Comment Parent { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}
