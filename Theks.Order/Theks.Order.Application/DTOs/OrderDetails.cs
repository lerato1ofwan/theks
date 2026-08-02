using System.ComponentModel.DataAnnotations;

namespace Theks.Order.Application.DTOs;

public record OrderDetals(
    [Required] Guid OrderId,
    [Required] Guid ProductId,
    [Required] Guid ClientId,
    [Required] string Name,
    [Required] string LastName,
    [Required, EmailAddress] string EmailAddress,
    [Required] string ContactNumber, 
    [Required] string Address, 
    [Required] string ProductName, 
    [Required] int Quantity,
    [Required, DataType(DataType.Currency)] decimal UnitPrice, 
    [Required, DataType(DataType.Currency)] decimal TotalPrice,
    [Required] DateTime OrderDate
);