namespace Theks.Order.Application.DTOs.Mappings;

public static class OrderMapper
{
    public static Domain.Entities.Order ToEntity(Order order) => new()
    {
        ProductId = order.ProductId,
        ClientId = order.ClientId,
        Quantity = order.Quantity
    };

    public static Order FromEntity(Domain.Entities.Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return new Order(
            order.Id,
            order.ProductId,
            order.ClientId,
            order.Quantity,
            order.CreatedDate);
    }

    public static IEnumerable<Order> FromEntityList(IEnumerable<Domain.Entities.Order>? orders)
    {
        if (orders is null)
        {
            return Enumerable.Empty<Order>();
        }

        return orders.Select(FromEntity).ToList();
    }
}