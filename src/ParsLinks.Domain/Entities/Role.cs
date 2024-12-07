using Microsoft.AspNetCore.Identity;

namespace ParsLinks.Domain.Entities;

public class Role : IdentityRole<Guid>, IAuditEntity
{

    public Role()
    {
    }

    public Role(string roleName) : base(roleName)
    {
        NormalizedName = roleName.Normalize().ToUpper();
    }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();

    #region AuditEntity

    public DateTimeOffset? CreatedDate { get; set; } = DateTime.UtcNow;
    public string? CreatedUserId { get; set; }
    public string? CreatedIpAddress { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }
    public string? ModifiedUserId { get; set; }

    public string? ModifiedIpAddress { get; set; }

    #endregion


}
