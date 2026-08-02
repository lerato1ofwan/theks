using Theks.Identity.Application.DTOs;
using Theks.Shared.Responses;

namespace Theks.Identity.Application.Interfaces;

public interface IUserService
{
    Task<Response> RegisterAsync(ApplicationUser applicationUser, CancellationToken token);
    Task<Response> LoginAsync(Login login, CancellationToken token);
    Task<User> GetUserAsync(Guid userId, CancellationToken token);
}