namespace DistributedTransactionsApi.Core;

public class CorsMiddleware
{
    private readonly RequestDelegate _next;

    public CorsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        return BeginInvoke(context);
    }

    private Task BeginInvoke(HttpContext context)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
        context.Response.Headers.Add("Access-Control-Allow-Headers",
            new[] { "Origin, X-Requested-With, Content-Type, Accept" });
        context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
        context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });

        return _next.Invoke(context);
    }
}

public static class CorsMiddlewareExtensions
{
    public static IApplicationBuilder UseApplicationCors(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorsMiddleware>();
    }
}