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
            var productResult = await GetByAsync(_ => _.Name!.Equals(entity.Name, StringComparison.OrdinalIgnoreCase));
            if (productResult is not null && !string.IsNullOrWhiteSpace(productResult.Name))
            {
                return new Response(false, $"Product: {entity.Name} already exists");
            }

            var product = (await dbContext.Products.AddAsync(entity)).Entity;
            await dbContext.SaveChangesAsync();

            if (product is not null)
            {
                return new Response(true, $"Product: {product.Name} added succesfully");
            }
            else
            {
                return new Response(false, $"Error occurred while adding: {entity.Name} ");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, $"Error occurred when adding: {entity.Name}");
        }
    }

    public async Task<Response> DeleteAsync(Domain.Entities.Product entity, CancellationToken cancellationToken = default)
    {
        try
        {
            var productResult = await FindByIdAsync(entity.Id);
            if (productResult is null)
            {
                return new Response(false, $"Product: {entity.Name} not found");
            }

            dbContext.Products.Remove(entity);
            await dbContext.SaveChangesAsync();

            return new Response(true, $"Product: {productResult.Name} deleted successfully");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, $"Error occurred when deleting: {entity.Name}");
        }
    }

    public async Task<Domain.Entities.Product?> FindByIdAsync(Guid id)
    {
        try
        {
            var productResult = await dbContext.Products.FirstOrDefaultAsync(_ => _.Id == id);
            return productResult;
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception($"Error occurred retrieving product with Id: {id}");
        }
    }

    public async Task<IList<Domain.Entities.Product>> GetAllAsync()
    {
        try
        {
            var productResult = await dbContext.Products.AsNoTracking().ToListAsync();

            return productResult;
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception($"Error occurred retrieving products");
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
            throw new Exception($"Error occurred retrieving products");
        }
    }

    public async Task<Response> UpdateAsync(Domain.Entities.Product entity, CancellationToken cancellationToken = default)
    {
        try
        {
            var productResult = await FindByIdAsync(entity.Id);
            if (productResult is null)
            {
                return new Response(false, $"Product: {entity.Name} not found");
            }

            dbContext.Entry(productResult).State = EntityState.Detached;
            dbContext.Products.Update(entity);
            await dbContext.SaveChangesAsync();

            return new Response(true, $"Product: {productResult.Name} updated successfully");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, $"Error occurred when updating: {entity.Name}");
        }
    }
}