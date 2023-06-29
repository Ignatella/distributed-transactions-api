using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace DistributedTransactionsApi.Core;

internal static class ExceptionHandler
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/plain; charset=utf-8";

                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (exceptionHandlerFeature != null)
                {
                    if (exceptionHandlerFeature.Error is ApplicationException ex)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        
                        await context.Response.WriteAsync(ex.Message);
                        return;
                    }

                    await context.Response.WriteAsync("Internal server error");
                }
            });
        });

        return app;
    }
}