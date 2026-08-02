using System.Net.Http.Json;
using Polly.Registry;
using Theks.Order.Application.DTOs;
using Theks.Order.Application.Interfaces;

namespace Theks.Order.Infrastructure.Services;

public class UserService(
    HttpClient httpClient,
    ResiliencePipelineProvider<string> resiliencePipeline) : IUserService
{
    public async Task<ApplicationUser?> GetApplicationUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        // @Todo: clean this up later (use Grpc)
        var retryPipeline = resiliencePipeline.GetPipeline("Theks.Order.RetryPipeline");

        return await retryPipeline.ExecuteAsync(async ct =>
        {
            var response = await httpClient.GetAsync($"/api/user/{userId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<ApplicationUser>(cancellationToken: cancellationToken);
        }, cancellationToken);
    }
}
