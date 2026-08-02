using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Theks.Order.Application.Interfaces;
using Theks.Order.Infrastructure.Data;
using Theks.Shared.Logs;
using Theks.Shared.Responses;

namespace Theks.Order.Infrastructure.Repositories;

public class OrderRepository(
    ApplicationDbContext dbContext) : IOrderRepository
{
    // @Todo: Ensure this functionality in idempotent
    public async Task<Response> CreateAsync(Domain.Entities.Order entity, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = dbContext.Orders.Add(entity).Entity;
            await dbContext.SaveChangesAsync();

            return order.Id != default ?
                new Response(true, "Order placed successfully")
                : new Response(false, "Error occurred while placing order");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Error occurred while placing order");
        }
    }

    public async Task<Response> DeleteAsync(Domain.Entities.Order entity, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await FindByIdAsync(entity.Id);
            if (order is null)
            {
                return new Response(false, "Error occurred while deleting order");
            }

            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync();

            return new Response(true, "Order deleted successfully");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "Error occurred while deleting order");
        }
    }

    public async Task<Domain.Entities.Order?> FindByIdAsync(Guid id)
    {
        try
        {
            return await dbContext.Orders.FirstOrDefaultAsync(_ => _.Id == id);
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Error occurred while retrieving yout order", ex);
        }
    }

    public async Task<IList<Domain.Entities.Order>> GetAllAsync()
    {
        try
        {
            return await dbContext.Orders.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("Error occurred while retrieving your order", ex);
        }
    }

    public async Task<Domain.Entities.Order?> GetByAsync(
       Expression<Func<Domain.Entities.Order, bool>> predicate,
       CancellationToken cancellationToken = default)
    {
        try
        {
            return await dbContext.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);

            throw new Exception(
                "An error occurred while retrieving your order",
                ex);
        }
    }


    public async Task<IEnumerable<Domain.Entities.Order>> GetOrdersAsync(
        Expression<Func<Domain.Entities.Order, bool>> predicate)
    {
        try
        {
            return await dbContext.Orders
                .AsNoTracking()
                .Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);

            throw new Exception(
                "An error occurred while retrieving your orders",
                ex);
        }
    }

    public async Task<Response> UpdateAsync(Domain.Entities.Order entity, CancellationToken cancellationToken = default)
    {
           try
        {
            var order = await FindByIdAsync(entity.Id);
            if(order is null)
            {
                return new Response(false, "Order not found");
            }

            dbContext.Entry(order).State = EntityState.Detached;
            dbContext.Orders.Update(entity);
            await dbContext.SaveChangesAsync();

            return new Response(true, "Order updated successfully");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);

            throw new Exception(
                "An error occurred while updating your order",
                ex);
        }
    }
}