using Microsoft.EntityFrameworkCore;
using Theks.Identity.Application.Interfaces;
using Theks.Identity.Infrastructure.Data;
using Theks.Shared.Logs;

namespace Theks.Identity.Infrastructure.Repositories;

public class UserRepository(
    ApplicationDbContext dbContext) : IUserRepository
{
    public async Task CreateAsync(Domain.Entities.ApplicationUser user, CancellationToken cancellationToken)
    {
        try
        {
            dbContext.ApplicationUsers.Add(user);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var message = $"{nameof(UserRepository)}.{nameof(CreateAsync)} Failed: {ex.Message}";
            LogException.LogToConsole(message);
            LogException.LogToFile(message);
            throw new Exception(message, ex);
        }
    }

    public async Task<Domain.Entities.ApplicationUser?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ApplicationUsers.FindAsync(id);
    }

    public async Task<Domain.Entities.ApplicationUser?> GetByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken = default)
    {
        return await dbContext.ApplicationUsers.FirstOrDefaultAsync(_ => _.EmailAddress.ToLower() == emailAddress);
    }
}