namespace Theks.Product.Application.DTOs.Mappings;

public static class ProductMapper
{
    public static Domain.Entities.Product ToEntity(ProductDto product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Quantity = product.Quantity,
        Description = product.Description,
        Price = product.Price
    };

    public static (ProductDto?, IEnumerable<ProductDto>?) FromEntity(
        Domain.Entities.Product product, IEnumerable<Domain.Entities.Product>? products)
    {
        if (product is not null)
        {
            var singleProduct = new ProductDto(product!.Id, product.Name, product.Description, product.Price, product.Quantity);

            return (singleProduct, null);
        }

        if(products is not null)
        {
            var _products = products.Select(product =>
                new ProductDto(product!.Id, product.Name, product.Description, product.Price, product.Quantity)
            ).ToList();

            return (null, _products);
        }

        return (null, null);
    }
}