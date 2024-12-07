using AutoMapper;
using ParsLinks.Api.Extensions;
using ParsLinks.Api.Services;
using ParsLinks.DataAccess.DbContext;
using ParsLinks.Domain.Entities;
using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.Models;
using EAllyfe.Api.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AuthService : IAuthService
{
    private readonly AppDbContext _appDbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AppConfig _config;
    private readonly IMapper _mapper;
    private readonly IJsonSerializer _jsonSerializer;

    public AuthService(
        AppDbContext appDbContext,
        IPasswordHasher<User> passwordHasher,
        IConfiguration config,
        IMapper mapper,
        IJsonSerializer jsonSerializer)
    {
        _appDbContext = appDbContext;
        _passwordHasher = passwordHasher;
        _config = config.Get<AppConfig>()!;
        _mapper = mapper;
        _jsonSerializer = jsonSerializer;
    }

    public async Task<ServiceResult<TokenResponse>> LoginAsync(LoginByEmailRequest request, HttpContext context, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users
            .Where(x => x.NormalizedEmail != null && x.NormalizedEmail.Equals(request.Email.Normalize().ToUpper()))
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password) == PasswordVerificationResult.Failed)
            return ServiceResult<TokenResponse>.Failure("Incorrect UserName or password.", StatusCodes.Status401Unauthorized);

        if (!user.IsActive)
            return ServiceResult<TokenResponse>.Failure("User Not Active. Please contact the administrator.", StatusCodes.Status401Unauthorized);

        // Get device information
        var (browser, os, deviceType, ipAddress, userAgent) = context.GetDeviceInfo();
        var deviceIdentifier = context.TraceIdentifier;

        var session = new UserSession
        {
            UserId = user.Id,
            UserAgent = userAgent,
            Device = browser,
            Platform = os,
            DeviceIdentifier = deviceIdentifier,
            LoginProvider = "IdentityServer",
            SessionIpAddress = ipAddress,
            StartDate = DateTime.UtcNow,
            BuildVersion = "v1",
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpires = DateTime.UtcNow.AddDays(_config.TenantJwtSettings.RefreshTokenExpiryInDay),

        };

        await _appDbContext.UserSessions.AddAsync(session, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        var claims = new List<Claim>
        {
            new Claim(AppClaimTypes.SessionId, session.Id.ToString()),
            new Claim(AppClaimTypes.UserId, session.UserId.ToString())
        };
        var token = new TokenResponse
        {
            Scheme = "Identity",
            AccessToken = JwtHelper.GenerateJwtToken(claims, _config.TenantJwtSettings),
            AccessTokenExpires = DateTimeOffset.UtcNow.AddMinutes(_config.TenantJwtSettings.TokenExpiryInMinutes).ToUnixTimeSeconds(),
            RefreshToken = session.RefreshToken,
            RefreshTokenExpires = session.RefreshTokenExpires.ToUnixTimeSeconds()
        };

        return ServiceResult<TokenResponse>.Success(token);
    }

    public async Task<ServiceResult<TokenResponse>> RefreshTokenAsync(string refreshToken, HttpContext context, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var session = await _appDbContext.UserSessions
            .Where(x => x.RefreshToken == refreshToken
                && x.IsRevoked == false
                && x.RefreshTokenExpires > now)
            .FirstOrDefaultAsync(cancellationToken);

        if (session == null)
        {
            return ServiceResult<TokenResponse>.Failure("Refresh Token is expired.");
        }




        session.RevokedDateTime = DateTime.UtcNow;
        session.RevokedByIp = context.GetClientIpAddress();
        session.ReplacedByToken = session.RefreshToken;
        session.RefreshToken = Guid.NewGuid().ToString();

        _appDbContext.UserSessions.Update(session);
        await _appDbContext.SaveChangesAsync(cancellationToken);


        var claims = new List<Claim>
        {
            new Claim(AppClaimTypes.SessionId, session.Id.ToString()),
            new Claim(AppClaimTypes.UserId, session.UserId.ToString())
        };

        var token = new TokenResponse
        {
            AccessToken = JwtHelper.GenerateJwtToken(claims, _config.TenantJwtSettings),
            AccessTokenExpires = now.AddMinutes(_config.TenantJwtSettings.TokenExpiryInMinutes).ToUnixTimeSeconds(),
            RefreshToken = session.RefreshToken,
            RefreshTokenExpires = session.RefreshTokenExpires.ToUnixTimeSeconds(),
        };




        return ServiceResult<TokenResponse>.Success(token);


    }

    public async Task<ServiceResult<TokenResponse>> RegisterAsync(RegisterRequest request, HttpContext context, CancellationToken cancellationToken)
    {
        // Check if email already exists
        var isDuplicate = await _appDbContext.Users
            .AnyAsync(a => a.NormalizedEmail != null
                && a.NormalizedEmail.Equals(request.Email.Normalize().ToUpper()),
                cancellationToken);

        if (isDuplicate)
            return ServiceResult<TokenResponse>.Failure("Email already exists.", StatusCodes.Status409Conflict);

        // Map the incoming request to a User entity
        var user = _mapper.Map<User>(request);
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        // Add the user to the database
        var userEntity = await _appDbContext.Users.AddAsync(user, cancellationToken);

        // Assign base and client roles
        var baseRole = await _appDbContext.Roles
            .AsNoTracking()
            .Where(x => x.Name != null && x.Name.Equals(AppRoles.Base))
            .FirstOrDefaultAsync(cancellationToken);

        if (baseRole != null)
        {
            await _appDbContext.UserRoles
                .AddAsync(new UserRole(userEntity.Entity.Id, baseRole.Id), cancellationToken);
        }

        var clientRole = await _appDbContext.Roles
            .AsNoTracking()
            .Where(x => x.Name != null && x.Name.Equals(AppRoles.User))
            .FirstOrDefaultAsync(cancellationToken);

        if (clientRole != null)
        {
            await _appDbContext.UserRoles
                .AddAsync(new UserRole(userEntity.Entity.Id, clientRole.Id), cancellationToken);
        }

        // Set user settings
        user.Settings = _jsonSerializer.Serialize(new
        {
            Culture = "en-US",
            DarkMode = false,
            RightToLeft = false
        });

        await _appDbContext.SaveChangesAsync(cancellationToken);

        // Get device information and IP address
        var (browser, os, deviceType, ipAddress, userAgent) = context.GetDeviceInfo();
        var deviceIdentifier = context.TraceIdentifier;

        // Create a session for the user
        var session = new UserSession
        {
            UserId = user.Id,
            UserAgent = userAgent,
            Device = browser,
            Platform = os,
            DeviceIdentifier = deviceIdentifier,
            LoginProvider = "IdentityServer",
            SessionIpAddress = ipAddress,
            StartDate = DateTime.UtcNow,
            BuildVersion = "v1",
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpires = DateTime.UtcNow.AddDays(_config.TenantJwtSettings.RefreshTokenExpiryInDay),
        };

        await _appDbContext.UserSessions.AddAsync(session, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);




        var claims = new List<Claim>
        {
            new Claim(AppClaimTypes.SessionId, session.Id.ToString()),
            new Claim(AppClaimTypes.UserId, session.UserId.ToString())
        };

        // Return token response
        var tokenResponse = new TokenResponse
        {
            Scheme = "Identity",
            AccessToken = JwtHelper.GenerateJwtToken(claims, _config.TenantJwtSettings),
            AccessTokenExpires = DateTimeOffset.UtcNow.AddMinutes(_config.TenantJwtSettings.TokenExpiryInMinutes).ToUnixTimeSeconds(),
            RefreshToken = session.RefreshToken,
            RefreshTokenExpires = session.RefreshTokenExpires.ToUnixTimeSeconds()
        };

        return ServiceResult<TokenResponse>.Success(tokenResponse);
    }

    public async Task<ServiceResult> LogoutAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var session = await _appDbContext.UserSessions
            .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken);

        if (session == null)
        {
            return ServiceResult.Failure("Session not found.");
        }

        _appDbContext.UserSessions.Remove(session);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success();
    }

    public async Task<ServiceResult<List<ClaimResponse>>> GetClaimsPrincipalsAsync(HttpContext context, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        // Check session
        var sessionId = context.User.GetSessionId();

        if (string.IsNullOrWhiteSpace(sessionId))
        {
            return ServiceResult<List<ClaimResponse>>.Failure("Unauthorized.", StatusCodes.Status401Unauthorized);
        }

        var session = await _appDbContext.UserSessions
            .AsNoTracking()
            .Where(x => x.Id == Guid.Parse(sessionId)
                && !x.IsRevoked
                && x.RefreshTokenExpires > now)
            .FirstOrDefaultAsync(cancellationToken);


        if (session == null)
        {
            return ServiceResult<List<ClaimResponse>>.Failure("Unauthorized.", StatusCodes.Status401Unauthorized);
        }

        var claims = await JwtHelper.GetClaimsAsync(session.Id, session.UserId, _appDbContext, cancellationToken);

        var response = claims.Select(s => new ClaimResponse
        {
            Type = s.Type,
            Value = s.Value
        }).ToList();

        return ServiceResult<List<ClaimResponse>>.Success(response);
    }

    public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.Password))
        {
            return ServiceResult.Failure("Invalid Request.", StatusCodes.Status400BadRequest);
        }

        var user = await _appDbContext.Users
            .Where(x => x.PasswordToken == request.Token)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return ServiceResult.Failure("Invalid Token.", StatusCodes.Status404NotFound);
        }

        if (user.PasswordTokenExpires < DateTime.UtcNow)
        {
            return ServiceResult.Failure("Token Expired.", StatusCodes.Status400BadRequest);
        }

        // Reset password and user info
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        user.SecurityStamp = Guid.NewGuid().ToString();
        user.PasswordToken = null;
        user.PasswordTokenExpires = null;
        user.PasswordResetAt = DateTime.UtcNow;
        await _appDbContext.SaveChangesAsync(cancellationToken);




        return ServiceResult.Success();
    }


    public async Task<ServiceResult> RequestPasswordResetAsync(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(email))
        {
            return ServiceResult.Failure("Invalid Request.", StatusCodes.Status400BadRequest);
        }

        // Find user by normalized email
        var user = await _appDbContext.Users
            .Where(x => x.NormalizedEmail == email.Normalize().ToUpper())
            .FirstOrDefaultAsync(cancellationToken);

        // Avoid sending "user not found" response, continue processing silently if user is not found
        if (user != null)
        {
            user.PasswordToken = JwtHelper.GenerateRandomCode();
            user.PasswordTokenExpires = DateTime.UtcNow.AddHours(1);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            // Retrieve reset password email template
        }

        return ServiceResult.Success();
    }



}


