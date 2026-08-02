using Theks.Identity.Application.DTOs;

namespace Theks.Identity.Application.Interfaces;

public interface ITokenService
{
      Task<AccessTokenResult> GenerateAccessTokenAsync(
        Domain.Entities.ApplicationUser user,
        CancellationToken cancellationToken = default);
}