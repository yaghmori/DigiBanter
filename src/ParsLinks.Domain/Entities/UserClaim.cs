using Microsoft.AspNetCore.Identity;

namespace ParsLinks.Domain.Entities;

public class UserClaim : IdentityUserClaim<Guid>, IAuditEntity
{
    public UserClaim()
    {

    }
    public UserClaim(Guid userId, string claimType, string claimValue)
    {
        UserId = userId;
        ClaimType = claimType;
        ClaimValue = claimValue;

    }

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
