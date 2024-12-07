using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParsLinks.Domain.Entities;

public class UserSession : IAuditEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 1)]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? ConnectionID { get; set; }
    public string? LoginProvider { get; set; } = default!;
    public string? BuildVersion { get; set; } = default!;
    public string? DeviceIdentifier { get; set; } = default!;
    public string? Name { get; set; } = default!;
    public string? UserAgent { get; set; } = default!;
    public string? Device { get; set; } = default!;
    public string? Platform { get; set; } = default!;
    public string? RefreshToken { get; set; } = default!;
    public DateTimeOffset RefreshTokenExpires { get; set; }

    public DateTimeOffset? StartDate { get; set; }
    public string? SessionIpAddress { get; set; }
    public virtual User User { get; set; }

    public bool IsRevoked { get; set; }
    public DateTimeOffset? RevokedDateTime { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }

    public bool IsActive => !IsRevoked && !IsExpired;
    public bool IsExpired => DateTime.UtcNow >= RefreshTokenExpires;





    #region AuditEntity
    public DateTimeOffset? CreatedDate { get; set; } = DateTime.UtcNow;
    public string? CreatedUserId { get; set; }
    public string? CreatedIpAddress { get; set; }

    public DateTimeOffset? ModifiedDate { get; set; }
    public string? ModifiedUserId { get; set; }
    public string? ModifiedIpAddress { get; set; }
    #endregion







}
