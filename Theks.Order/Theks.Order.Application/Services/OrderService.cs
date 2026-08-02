using Theks.Order.Application.DTOs;
using Theks.Order.Application.DTOs.Mappings;
using Theks.Order.Application.Interfaces;
using Theks.Shared.Responses;

namespace Theks.Order.Application.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IProductService productService,
    IUserService userService)
    : IOrderService
{
    public async Task<OrderDetals?> GetOrderDetalsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.FindByIdAsync(orderId);
        if (order is null || order.Id == default)
        {
            return null;
        }

        var productDto = await productService.GetProductAsync(order.ProductId, cancellationToken);
        var applicationUserDto = await userService.GetApplicationUserAsync(order.ClientId, cancellationToken);

        if (productDto is null || applicationUserDto is null) return null;

        return new OrderDetals(
            order.Id,
            productDto.Id,
            applicationUserDto.Id,
            applicationUserDto.FirstName,
            applicationUserDto.LastName,
            applicationUserDto.EmailAddress,
            applicationUserDto.ContactNumber,
            applicationUserDto.Address,
            productDto.Name,
            order.Quantity,
            productDto.Quantity,
            productDto.Quantity * order.Quantity,
            order.CreatedDate
        );
    }

  #region Repository abstractions

    public async Task<IEnumerable<DTOs.Order>> GetOrdersAsync()
    {
        var domainOrders = await orderRepository.GetAllAsync();

        return OrderMapper.FromEntityList(domainOrders);
    }

    public async Task<DTOs.Order?> GetOrderByIdAsync(Guid id)
    {
        var domainOrder = await orderRepository.FindByIdAsync(id);
        if(domainOrder is null)
        {
            return null;
        }

        return OrderMapper.FromEntity(domainOrder);
    }

    public async Task<Response> CreateOrderAsync(DTOs.Order order)
    {
        var orderEntity = OrderMapper.ToEntity(order);
        var result = await orderRepository.CreateAsync(orderEntity);

        return result;
    }

    public async Task<Response> UpdateOrderAsync(DTOs.Order order)
    {
        var orderEntity = OrderMapper.ToEntity(order);
        var result = await orderRepository.UpdateAsync(orderEntity);

        return result;
    }
    public async Task<Response> DeleteOrderAsync(DTOs.Order order)
    {
        var orderEntity = OrderMapper.ToEntity(order);
        var result = await orderRepository.DeleteAsync(orderEntity);

        return result;
    }

    public async Task<IEnumerable<DTOs.Order>> GetOrdersByClientId(Guid clientId)
    {
        var domainOrders = await orderRepository.GetOrdersAsync(_ => _.ClientId == clientId);

        if (domainOrders == null || !domainOrders.Any())
        {
            return Enumerable.Empty<DTOs.Order>();
        }

        return OrderMapper.FromEntityList(domainOrders);

    }

    #endregion
}
