using Theks.Product.Application.Interfaces;
using Theks.Product.Application.DTOs.Mappings;
using Theks.Shared.Responses;

namespace Theks.Product.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<Response> CreateAsync(DTOs.Product dto, CancellationToken cancellationToken = default)
    {
        var existingProduct = await productRepository.GetByAsync(p => p.Name!.ToLower() == dto.Name.ToLower(), cancellationToken);
        if (existingProduct is not null)
        {
            return new Response(false, $"Product: {dto.Name} already exists.");
        }

        var entity = ProductMapper.ToEntity(dto);
        var result = await productRepository.CreateAsync(entity, cancellationToken);
        
        return result;
    }

    public async Task<Response> UpdateAsync(DTOs.Product dto, CancellationToken cancellationToken = default)
    {
        var existingProduct = await productRepository.FindByIdAsync(dto.Id);
        if (existingProduct is null)
        {
            return new Response(false, $"Product with ID {dto.Id} not found.");
        }

        var entity = ProductMapper.ToEntity(dto);
        return await productRepository.UpdateAsync(entity, cancellationToken);
    }

    public async Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existingProduct = await productRepository.FindByIdAsync(id);
        if (existingProduct is null)
        {
            return new Response(false, "Product not found.");
        }

        return await productRepository.DeleteAsync(existingProduct, cancellationToken);
    }

    public async Task<DTOs.Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await productRepository.FindByIdAsync(id);
        return entity is null ? null : ProductMapper.FromEntity(entity);
    }

    public async Task<IList<DTOs.Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await productRepository.GetAllAsync();
        return ProductMapper.FromEntityList(entities).ToList();
    }
}
