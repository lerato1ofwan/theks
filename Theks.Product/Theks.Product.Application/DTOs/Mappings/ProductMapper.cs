namespace Theks.Product.Application.DTOs.Mappings;

public static class ProductMapper
{
    public static Domain.Entities.Product ToEntity(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Quantity = product.Quantity,
        Description = product.Description,
        Price = product.Price
    };

    public static (Product?, IEnumerable<Product>?) FromEntity(
        Domain.Entities.Product product, IEnumerable<Domain.Entities.Product>? products)
    {
        if (product is not null)
        {
            var singleProduct = new Product(product!.Id, product.Name, product.Description, product.Price, product.Quantity);

            return (singleProduct, null);
        }

        if(products is not null)
        {
            var _products = products.Select(product =>
                new Product(product!.Id, product.Name, product.Description, product.Price, product.Quantity)
            ).ToList();

            return (null, _products);
        }

        return (null, null);
    }
}