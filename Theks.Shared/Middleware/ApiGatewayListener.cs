using Microsoft.AspNetCore.Http;

namespace Theks.Shared.Middleware;

public class ApiGatewayListener(RequestDelegate next)
{
    private const string _apiGatewayHeaderName = "Api-Gateway";
    private const string _unavailableServiceMessage = "Service is unavailable";
    public async Task InvokeAsync(HttpContext context)
    {
        var signedHeader = context.Request.Headers[_apiGatewayHeaderName];

        // @Hint: Request not sent by the api gateway
        if (signedHeader.FirstOrDefault() is null)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync(_unavailableServiceMessage);
            return;
        }
        else
        {
            await next(context);
        }
    }
}