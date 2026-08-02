using System.Net.Http.Json;
using Polly.Registry;
using Theks.Order.Application.DTOs;
using Theks.Order.Application.Interfaces;

namespace Theks.Order.Infrastructure.Services;

public class ProductService(
    HttpClient httpClient,
    ResiliencePipelineProvider<string> resiliencePipeline) : IProductService
{
    public async Task<Product?> GetProductAsync(Guid productId, CancellationToken cancellationToken)
    {
        var retryPipeline = resiliencePipeline.GetPipeline("Theks.Order.RetryPipeline");

        return await retryPipeline.ExecuteAsync(async ct =>
        {
            var response = await httpClient.GetAsync($"/api/products/{productId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<Product>(cancellationToken: cancellationToken);
        }, cancellationToken);
    }
}
