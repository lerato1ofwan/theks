using Serilog;
using Theks.Identity.Application.DTOs;
using Theks.Identity.Application.DTOs.Mappings;
using Theks.Identity.Application.Interfaces;
using Theks.Shared.Responses;

namespace Theks.Identity.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    : IUserService
{
    public async Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetAsync(userId, cancellationToken);
        if(user is null)
        {
            return null!;
        }

        var applicationUserDto = UserMapper.FromEntity(user);

        return applicationUserDto;
    }

    public async Task<Response> LoginAsync(Login login, CancellationToken cancellationToken = default)
    {
        var applicationUser = await userRepository.GetByEmailAddressAsync(login.EmailAddress);
        if(applicationUser is null)
        {
            return new Response(false, "Login failed: Incorrect email address or password");
        }

        bool verifyPassword = passwordHasher.Verify(login.Password, applicationUser.Password);
        if (!verifyPassword)
        {
            return new Response(false, "Login failed: Incorrect email address or password");
        }

        var token = await tokenService.GenerateAccessTokenAsync(
            applicationUser,
            cancellationToken);


        throw new NotImplementedException();
    }

    // @Todo: Verify registration: Integrate email or messaging service for complete registration feature

    public async Task<Response> RegisterAsync(ApplicationUser applicationUser, CancellationToken cancellationToken = default)
    {
        var applicationUserDto = await userRepository.GetByEmailAddressAsync(applicationUser.EmailAddress);
        if(applicationUserDto is null)
        {
            return new Response(false, "If an account can be created with this email address, you'll receive an email with the next steps.");
        }

        var domainUser = ApplicationUserMapper.ToEntity(applicationUser, passwordHasher);
        
        await userRepository.CreateAsync(domainUser, cancellationToken);

        return new Response(true, "Registation successful. Please check your email to verify your account");

    }
}