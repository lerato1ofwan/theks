using Theks.Identity.Application.Interfaces;

namespace Theks.Identity.Application.DTOs.Mappings;

public static class ApplicationUserMapper
{
    public static Domain.Entities.ApplicationUser ToEntity(ApplicationUser user, IPasswordHasher passwordHasher)
    {
        // @Hint: Preserve existing Id for updates; for creates the domain entity will generate a new Id.
        var entity = new Domain.Entities.ApplicationUser
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailAddress = user.EmailAddress,
            ContactNumber = user.ContactNumber,
            Password = passwordHasher.Hash(user.Password),
            Role = user.Role,
            Address = user.Address
        };

        if (user.Id != Guid.Empty)
        {
            entity.Id = user.Id;
        }

        return entity;
    }

    public static ApplicationUser FromEntity(Domain.Entities.ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new ApplicationUser(
            user.Id,
            user.FirstName,
            user.LastName,
            user.EmailAddress,
            user.ContactNumber,
            user.Password,
            user.Role,
            user.Address);
    }

    public static IEnumerable<ApplicationUser> FromEntityList(IEnumerable<Domain.Entities.ApplicationUser>? users)
    {
        if (users is null)
        {
            return Enumerable.Empty<ApplicationUser>();
        }

        return users.Select(FromEntity).ToList();
    }
}