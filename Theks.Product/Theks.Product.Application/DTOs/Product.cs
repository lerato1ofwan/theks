using System.ComponentModel.DataAnnotations;

namespace Theks.Product.Application.DTOs;

public record Product(
    Guid Id,
    [Required] string Name,
    string Description,
    [Required, DataType(DataType.Currency)] decimal Price,
    [Required, Range(1, int.MaxValue)] int Quantity
);