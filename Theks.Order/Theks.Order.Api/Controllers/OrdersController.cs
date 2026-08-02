using Microsoft.AspNetCore.Mvc;
using Theks.Order.Application.Interfaces;
using Theks.Shared.Responses;

namespace Theks.Order.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(
    IOrderService ordersService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Application.DTOs.Order>>> GetOrders()
    {
        var orders = await ordersService.GetOrdersAsync();

        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Application.DTOs.Order>> GetOrderAsync(Guid id)
    {
        var order = await ordersService.GetOrderByIdAsync(id);
        if (order is null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Order Not Found",
                Detail = $"The order with ID '{id}' was not found",
                Instance = HttpContext.Request.Path
            });
        }
        return Ok(order);
    }

    [HttpGet("client/{id:guid}")]
    public async Task<ActionResult<Application.DTOs.Order>> GetClientOrdersAsync(Guid id)
    {
        var order = await ordersService.GetOrdersByClientId(id);
        return Ok(order);
    }

    [HttpGet("{id:guid}/details")]
    public async Task<ActionResult<Application.DTOs.Order>> GetOrderDetailsAsync(Guid id)
    {
        var order = await ordersService.GetOrderDetalsAsync(id);
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<Response>> CreateOrderAsync([FromBody] Application.DTOs.Order order)
    {
        var response = await ordersService.CreateOrderAsync(order);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<Response>> UpdateOrderAsync([FromBody] Application.DTOs.Order order)
    {
        var response = await ordersService.UpdateOrderAsync(order);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<ActionResult<Response>> DeleteOrderAsync([FromBody] Application.DTOs.Order order)
    {
        var response = await ordersService.DeleteOrderAsync(order);
        return Ok(response);
    }
}