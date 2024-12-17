using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Extensions;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ParsLinks.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal)
        => principal.FindFirstValue(AppClaimTypes.Email);


    public static string? GetTenantId(this ClaimsPrincipal principal)
        => principal.FindFirstValue(AppClaimTypes.TenantId);
    public static string? GetTimeZoneId(this ClaimsPrincipal principal)
    => principal.FindFirstValue(AppConstants.TimeZoneId);


    public static string? GetUserId(this ClaimsPrincipal principal)
        => principal?.FindFirstValue(AppClaimTypes.UserId);



    public static string? GetSessionId(this ClaimsPrincipal principal)
        => principal.FindFirstValue(AppClaimTypes.SessionId);


    public static string? GetSecurityStamp(this ClaimsPrincipal principal)
        => principal?.FindFirstValue(AppClaimTypes.SecurityStamp);

    public static DateTimeOffset GetExpiration(this ClaimsPrincipal principal) =>
        DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(
            principal.FindFirstValue(AppClaimTypes.Expiration)));

    private static string? FindFirstValue(this ClaimsPrincipal principal, string claimType) =>
        principal is null
            ? throw new ArgumentNullException(nameof(principal))
            : principal.FindFirst(claimType)?.Value?.ToLowerInvariant();
}
