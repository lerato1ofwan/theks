using System.ComponentModel.DataAnnotations;

namespace Theks.Identity.Application.DTOs;

public record User(
    Guid Id,
    [Required] string FirstName,
    [Required] string LastName,
    [Required, EmailAddress] string EmailAddress,
    [Required] string ContactNumber,
    [Required] string Role,
    [Required] string Address
);