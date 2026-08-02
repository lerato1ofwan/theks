using Theks.Order.Application.DTOs;

namespace Theks.Order.Application.Interfaces;

public interface IProductService
{
    Task<Product?> GetProductAsync(Guid productId, CancellationToken cancellationToken);
}
