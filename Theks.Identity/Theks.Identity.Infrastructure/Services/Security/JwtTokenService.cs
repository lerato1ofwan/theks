using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Theks.Identity.Application.DTOs;
using Theks.Identity.Application.Interfaces;
using Theks.Identity.Infrastructure.Configurations;

namespace Theks.Identity.Infrastructure.Services.Security;

public sealed class JwtTokenService(
    AuthenticationOptions options) : ITokenService
{
    public Task<AccessTokenResult> GenerateAccessTokenAsync(
     Domain.Entities.ApplicationUser user,
     CancellationToken cancellationToken = default)
    {
        var key = Encoding.UTF8.GetBytes(options.Key);

        var securityKey = new SymmetricSecurityKey(key);

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(120);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.EmailAddress),
            new(JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        var accessToken = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return Task.FromResult(
            new AccessTokenResult(
                accessToken,
                expires));
    }
}