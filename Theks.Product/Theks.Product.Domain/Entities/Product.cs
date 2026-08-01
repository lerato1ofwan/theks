namespace Theks.Product.Domain.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
}