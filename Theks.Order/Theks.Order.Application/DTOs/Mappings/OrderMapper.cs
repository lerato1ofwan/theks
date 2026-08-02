namespace Theks.Order.Application.DTOs.Mappings;

public static class OrderMapper
{
    public static Domain.Entities.Order ToEntity(Order order)
    {
        // @Hint: Preserve existing Id for updates; for creates the domain entity will generate a new Id.
        var entity = new Domain.Entities.Order
        {
            ProductId = order.ProductId,
            ClientId = order.ClientId,
            Quantity = order.Quantity
        };

        if (order.Id != Guid.Empty)
        {
            entity.Id = order.Id;
        }

        return entity;
    }

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