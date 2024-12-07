using DigiBanter.DataAccess.DbContext;
using DigiBanter.Domain.Entities;
using DigiBanter.Shared.Constatns;
using DigiBanter.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DigiBanter.Shared.Helpers;

namespace EAllyfe.Api.Helpers;

public static class JwtHelper
{
    public static async Task<List<Claim>> GetClaimsAsync(Guid sessionId, Guid userId, AppDbContext appDbContext, CancellationToken cancellationToken = default)
    {
        //RoleClaims
        var roles = await appDbContext.UserRoles
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(s => new Role
            {
                Id = s.Role.Id,
                Name = s.Role.Name,
            })
            .ToListAsync(cancellationToken);



        var rClaims = await appDbContext.RoleClaims
            .AsNoTracking()
            .Where(x => roles.Select(s => s.Id).Contains(x.RoleId))
            .Select(s => new Claim(s.ClaimType, s.ClaimValue))
            .ToListAsync(cancellationToken);

        var roleClaims = new List<Claim>();

        roleClaims.AddRange(roles.Select(s => new Claim(ClaimTypes.Role, s.Name)).ToList());
        roleClaims.AddRange(rClaims);


        //userClaims
        var userClaims = new List<Claim>();

        var permissions = await appDbContext.UserClaims
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(s => new Claim(s.ClaimType, s.ClaimValue))
            .ToListAsync(cancellationToken);



        userClaims.Add(new Claim(AppClaimTypes.SessionId, sessionId.ToString()));
        userClaims.Add(new Claim(AppClaimTypes.UserId, userId.ToString()));

        userClaims.AddRange(permissions);



        var claims = new List<Claim>()
            .Union(roleClaims, new ClaimComparer())
            .Union(userClaims, new ClaimComparer());

        return claims.ToList();
    }
    public static string GenerateJwtToken(List<Claim> claims, JwtSettings jwtSettings)
    {

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.TokenExpiryInMinutes),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)), SecurityAlgorithms.HmacSha256));

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenOptions);
    }

    public static string GenerateRandomCode()
    {
        return new Random().Next(0, 1000000).ToString("D6");
    }

}
