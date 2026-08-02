namespace Theks.Order.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid ProductId { get; set; }
    public Guid ClientId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
