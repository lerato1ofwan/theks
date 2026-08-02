using Theks.Order.Application.DTOs;
using Theks.Shared.Responses;

namespace Theks.Order.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<DTOs.Order>> GetOrdersByClientId(Guid clientId);
    Task<OrderDetals?> GetOrderDetalsAsync(Guid orderId, CancellationToken cancellationToken = default);

    // Repository Abstractions
    Task<IEnumerable<DTOs.Order>> GetOrdersAsync();
    Task<DTOs.Order?> GetOrderByIdAsync(Guid id);
    Task<Response> CreateOrderAsync(DTOs.Order order);
    Task<Response> UpdateOrderAsync(DTOs.Order order);
    Task<Response> DeleteOrderAsync(DTOs.Order order);
}