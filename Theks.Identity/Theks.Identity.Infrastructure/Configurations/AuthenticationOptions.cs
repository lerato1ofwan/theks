namespace Theks.Identity.Infrastructure.Configurations;

public sealed class AuthenticationOptions
{
    public required string Key { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
}