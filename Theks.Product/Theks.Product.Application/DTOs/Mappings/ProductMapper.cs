using Theks.Product.Application.DTOs;
using Theks.Product.Domain.Entities;

namespace Theks.Product.Application.DTOs.Mappings;

public static class ProductMapper
{
    /// <summary>
    /// Maps a single Domain Product Entity to an Application DTOs.Product.
    /// </summary>
    public static Product? FromEntity(Domain.Entities.Product? entity)
    {
        if (entity is null) return null;

        return new Product(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Price,
            entity.Quantity
        );
    }

    /// <summary>
    /// Maps an Application DTOs.Product to a Domain Product Entity.
    /// </summary>
    public static Domain.Entities.Product ToEntity(Product dto)
    {
        // Preserve existing Id for updates; for creates the domain entity will generate a new Id.
        var entity = new Domain.Entities.Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description,
            Quantity = dto.Quantity
        };

        if (dto.Id != Guid.Empty)
        {
            entity.Id = dto.Id;
        }

        return entity;
    }

    /// <summary>
    /// Projects an enumerable collection of Product Entities to an enumerable collection of ProductDtos.
    /// </summary>
    public static IEnumerable<DTOs.Product> FromEntityList(IEnumerable<Domain.Entities.Product>? entities)
    {
        if (entities is null) return Enumerable.Empty<Product>();

        // Using .Select provides an optimized, deferred-execution projection pipeline
        return entities.Select(entity => new Product(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Price,
            entity.Quantity
        ));
    }
}
