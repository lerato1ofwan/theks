using Theks.Identity.Domain.Entities;

namespace Theks.Identity.Application.Interfaces;

public interface IUserRepository
{
    Task CreateAsync(ApplicationUser user, CancellationToken cancellationToken);
    Task<ApplicationUser?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken = default);
}