namespace DigiBanter.Domain.Entities;

public interface IAuditEntity
{
    public DateTimeOffset? CreatedDate { get; set; }
    public string? CreatedUserId { get; set; }
    //public User? CreatedUser { get; set; }
    public string? CreatedIpAddress { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }
    public string? ModifiedUserId { get; set; }
    //public User? ModifiedUser { get; set; }

    public string? ModifiedIpAddress { get; set; }

}
