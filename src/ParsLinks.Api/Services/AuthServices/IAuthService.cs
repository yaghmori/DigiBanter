using ParsLinks.Api.Services;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;

public interface IAuthService
{
    Task<ServiceResult<List<ClaimResponse>>> GetClaimsPrincipalsAsync(HttpContext context, CancellationToken cancellationToken);
    Task<ServiceResult<TokenResponse>> LoginAsync(LoginByEmailRequest request, HttpContext context, CancellationToken cancellationToken);
    Task<ServiceResult> LogoutAsync(Guid sessionId, CancellationToken cancellationToken);
    Task<ServiceResult<TokenResponse>> RefreshTokenAsync(string refreshToken, HttpContext context, CancellationToken cancellationToken);
    Task<ServiceResult<TokenResponse>> RegisterAsync(RegisterRequest request, HttpContext context, CancellationToken cancellationToken);
    Task<ServiceResult> RequestPasswordResetAsync(string email, CancellationToken cancellationToken);
    Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken);
}