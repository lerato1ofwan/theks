using System.ComponentModel.DataAnnotations;

namespace Theks.Order.Application.DTOs;

public record ApplicationUser(
    Guid Id,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Address,
    [Required, EmailAddress] string EmailAddress,
    [Required] string ContactNumber,
    [Required] string Password,
    [Required] string Role
);