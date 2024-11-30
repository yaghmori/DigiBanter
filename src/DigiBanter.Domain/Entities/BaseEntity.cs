using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiBanter.Domain.Entities;

public class BaseEntity<TKey> : IAuditEntity, IBaseEnity<TKey> where TKey : IEquatable<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TKey Id { get; set; }
    public DateTimeOffset? CreatedDate { get; set; }
    public string? CreatedUserId { get; set; }
    public string? CreatedIpAddress { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }
    public string? ModifiedUserId { get; set; }
    public string? ModifiedIpAddress { get; set; }

    public bool IsEqual(TKey a, TKey b)
    {
        return a.Equals(b);
    }


}
