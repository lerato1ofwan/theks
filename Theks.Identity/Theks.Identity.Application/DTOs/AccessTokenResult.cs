namespace Theks.Identity.Application.DTOs;

public sealed record AccessTokenResult(
    string AccessToken,
    DateTime ExpiresAtUtc);