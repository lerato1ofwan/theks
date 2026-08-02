using Theks.Order.Application.DTOs;

namespace Theks.Order.Application.Interfaces;


public interface IUserService
{
    Task<ApplicationUser?> GetApplicationUserAsync(Guid userId, CancellationToken cancellationToken);
}
