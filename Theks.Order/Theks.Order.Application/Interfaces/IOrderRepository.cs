using System.Linq.Expressions;
using Theks.Shared.Interface;

namespace Theks.Order.Application.Interfaces;

public interface IOrderRepository: IGenericInterface<Domain.Entities.Order>
{
    public Task<IEnumerable<Domain.Entities.Order>> GetOrdersAsync(Expression<Func<Domain.Entities.Order, bool>> predicate);
}