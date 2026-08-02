namespace Theks.Identity.Application.DTOs;

public record Login(
    string EmailAddress,
    string Password
);