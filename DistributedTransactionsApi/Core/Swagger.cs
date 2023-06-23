using Microsoft.OpenApi.Models;

namespace DistributedTransactionsApi.Core;

internal static class Swagger
{
    public static IServiceCollection AddApplicationSwagger(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            var scheme = new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Default scheme",
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(@"https://yermakovich.com/identity/connect/authorize"),
                        TokenUrl = new Uri(@"https://yermakovich.com/identity/connect/token"),
                        Scopes = new Dictionary<string, string>()
                        {
                            { "roles", "roles" },
                            { "email", "email" },
                            { "profile", "profile" },
                            { "openid", "openid" },
                            { "distributed-transactions-api", "distributed-transactions-api" }
                        }
                    },
                },
                Type = SecuritySchemeType.OAuth2,
            };

            options.AddSecurityDefinition("OAuth", scheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Id = "OAuth", Type = ReferenceType.SecurityScheme }
                    },
                    new List<string> { }
                }
            });
        });

        return services;
    }

    // ReSharper disable once InconsistentNaming
    public static IApplicationBuilder UseApplicationSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwaggerUI(options =>
        {
            options.OAuthClientId("xbank");
            options.OAuthScopes("roles", "email", "profile", "openid", "distributed-transactions-api");
            options.OAuthUsePkce();
            options.EnablePersistAuthorization();
        });

        return app;
    }
}