using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Theks.Shared.Logs;

namespace Theks.Shared.Middleware;

public class GlobalException(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        string message = "Internal Server Error occurred. Try again.";
        int statusCode = (int)HttpStatusCode.InternalServerError;
        string title = "Error";

        try
        {
            await next(context);

            // @Hint: Too Many Requests
            if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
            {
                title = "Warning";
                message = "Too many requests made.";
                statusCode = StatusCodes.Status429TooManyRequests;

                await ModifyHeaderAsync(context, title, message, statusCode);
            }
            
            // @Hint: Unauthorized
            if(context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                title = "Error";
                message = "You are not authorized to access.";
                statusCode = StatusCodes.Status401Unauthorized;
                await ModifyHeaderAsync(context, title, message, statusCode);
            }

            // @Hint: Forbidden
            if(context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                title = "Error";
                message = "You are not allowed to access.";
                statusCode = StatusCodes.Status403Forbidden;
                await ModifyHeaderAsync(context, title, message, statusCode);
            }
        }
        catch (Exception ex)
        {
            // @Hint: Log initial exceptions
            LogException.LogExceptions(ex);

            // @Hint: Handle timeout exceptions
            if(ex is TaskCanceledException || ex is TimeoutException)
            {
                title = "Request timeout";
                message = "Request timeout. Try again.";
                statusCode = StatusCodes.Status408RequestTimeout;
                await ModifyHeaderAsync(context, title, message, statusCode);
            }

            // @Hint: Defaults on unhandled exceptions
            await ModifyHeaderAsync(context, title, message, statusCode);
        }
    }

    private static async Task ModifyHeaderAsync(HttpContext context, string title, string message, int statusCode)
    {
        context.Response.ContentType = "applications/json";
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new ProblemDetails()
            {
                Title = title,
                Detail = message,
                Status = statusCode
            }
         ));
    }
}