namespace Theks.Identity.Application.DTOs.Mappings;

public static class UserMapper
{
    public static User FromEntity(Domain.Entities.ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new User(
            user.Id,
            user.FirstName,
            user.LastName,
            user.EmailAddress,
            user.ContactNumber,
            user.Role,
            user.Address);
    }

    public static IEnumerable<User> FromEntityList(IEnumerable<Domain.Entities.ApplicationUser>? users)
    {
        if (users is null)
        {
            return Enumerable.Empty<User>();
        }

        return users.Select(FromEntity).ToList();
    }
}