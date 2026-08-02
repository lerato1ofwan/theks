using System.Linq.Expressions;
using Theks.Shared.Responses;

namespace Theks.Shared.Interface;

public interface IGenericInterface<T> where T : class
{
    Task<Response> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<Response> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<Response> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<IList<T>> GetAllAsync();
    Task<T?> FindByIdAsync(Guid id);
    Task<T?> GetByAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}