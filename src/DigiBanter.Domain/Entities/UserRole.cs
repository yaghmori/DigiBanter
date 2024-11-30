using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiBanter.Domain.Entities;

public class UserRole : IdentityUserRole<Guid>, IAuditEntity
{
    public UserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
    public UserRole()
    {

    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public virtual Role Role { get; set; }
    public virtual User User { get; set; }

    #region AuditEntity
    public DateTimeOffset? CreatedDate { get; set; } = DateTime.UtcNow;
    public string? CreatedUserId { get; set; }
    public string? CreatedIpAddress { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }
    public string? ModifiedUserId { get; set; }

    public string? ModifiedIpAddress { get; set; }
    #endregion
}
