using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Theks.Product.Application.Interfaces;
using Theks.Product.Infrastructure.Data;
using Theks.Shared.Logs;
using Theks.Shared.Responses;

namespace Theks.Product.Infrastructure.Repositories;

public class ProductRepository(ProductDbContext dbContext) : IProductRepository
{
    public async Task<Response> CreateAsync(Domain.Entities.Product entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Products.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(true, $"Product: {entity.Name} added successfully.");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "An error occurred while saving the product.");
        }
    }

    public async Task<Response> UpdateAsync(Domain.Entities.Product entity, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.Entry(entity).State = EntityState.Detached;

            dbContext.Products.Update(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(true, $"Product: {entity.Name} updated successfully.");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "An error occurred while updating the product.");
        }
    }

    public async Task<Response> DeleteAsync(Domain.Entities.Product entity, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.Products.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(true, $"Product: {entity.Name} deleted successfully.");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "An error occurred while deleting the product.");
        }
    }

    public async Task<Domain.Entities.Product?> FindByIdAsync(Guid id)
    {
        try
        {
            // @Hint: Prefer FindAsync over FirstOrDefaultAsync for tracking-heavy primary key lookups
            return await dbContext.Products.FindAsync(id);
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new InvalidOperationException($"Error retrieving product with ID: {id}", ex);
        }
    }

    public async Task<IList<Domain.Entities.Product>> GetAllAsync()
    {
        try
        {
            return await dbContext.Products.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new InvalidOperationException("Error retrieving products", ex);
        }
    }

    public async Task<Domain.Entities.Product?> GetByAsync(Expression<Func<Domain.Entities.Product, bool>> predicate, CancellationToken cancellationToken = default)
    {
        try
        {
            return await dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new InvalidOperationException("Error executing filter query on products", ex);
        }
    }
}
