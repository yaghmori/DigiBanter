using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace DigiBanter.Domain.Entities;
public class User : IdentityUser<Guid>, IAuditEntity
{

    public int PublicId { get; set; } //.ValueGeneratedOnAdd(); auto increment
    public string? Provider { get; set; }
    public string? ProviderId { get; set; }
    public string? DisplayName { get; set; }
    public string? ExternalProfileImageUrl { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? ProfileImagePath { get; set; }

    public string? Description { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
    public string? IdNumber { get; set; }

    public string? EmailToken { get; set; }
    public DateTimeOffset? EmailTokenExpires { get; set; }
    public string? PhoneNumberToken { get; set; }
    public DateTimeOffset? PhoneNumberTokenExpires { get; set; }
    public string? PasswordToken { get; set; }
    public DateTimeOffset? PasswordTokenExpires { get; set; }
    public DateTimeOffset? PasswordResetAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsOnline { get; set; } = false;

    public string? Data { get; set; } = string.Empty;
    public string? Settings { get; set; }
    public string? Profile { get; set; }



    public virtual ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();
    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();
    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();


    #region AuditEntity
    public DateTimeOffset? CreatedDate { get; set; } = DateTime.UtcNow;
    public string? CreatedUserId { get; set; }
    public string? CreatedIpAddress { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }
    public string? ModifiedUserId { get; set; }

    public string? ModifiedIpAddress { get; set; }
    #endregion

}
