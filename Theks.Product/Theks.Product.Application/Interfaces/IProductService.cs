using Theks.Shared.Responses;

namespace Theks.Product.Application.Interfaces;

public interface IProductService 
{
    Task<Response> CreateAsync(DTOs.Product dto, CancellationToken cancellationToken = default);
    Task<Response> UpdateAsync(DTOs.Product dto, CancellationToken cancellationToken = default);
    Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DTOs.Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<DTOs.Product>> GetAllAsync(CancellationToken cancellationToken = default);
}
