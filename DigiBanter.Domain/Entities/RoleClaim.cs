using Microsoft.AspNetCore.Identity;

namespace DigiBanter.Domain.Entities;

public class RoleClaim : IdentityRoleClaim<Guid>, IAuditEntity
{
    public override Guid RoleId { get; set; }
    public override string? ClaimType { get; set; }
    public override string? ClaimValue { get; set; }
    public virtual Role Role { get; set; }

    #region AuditEntity
    public DateTimeOffset? CreatedDate { get; set; } = DateTime.UtcNow;
    public string? CreatedUserId { get; set; }
    public string? CreatedIpAddress { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }
    public string? ModifiedUserId { get; set; }
    public string? ModifiedIpAddress { get; set; }

    #endregion

}
