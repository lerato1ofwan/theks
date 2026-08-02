using System.ComponentModel.DataAnnotations;

namespace Theks.Order.Application.DTOs;

public record Order(
    Guid Id,
    [Required] Guid ProductId,
    [Required] Guid ClientId,
    [Required, Range(1, int.MaxValue)] int Quantity,
    DateTime CreatedDate
);