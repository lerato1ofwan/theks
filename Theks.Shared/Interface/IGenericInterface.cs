using Theks.Shared.Responses;

namespace Theks.Shared.Interface;

public interface IGenericInterface<T> where T : class
{
    Task<Response> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<Response> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<Response> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<IList<T>> GetAllAsync();
    Task<T> FindByIdAsync(int id);
    Task<T> GetByAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default);
}