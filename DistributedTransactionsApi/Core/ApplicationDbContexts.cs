using DistributedTransactionsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DistributedTransactionsApi.Core;

internal static class ApplicationDbContexts
{
    public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BankMasterContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Master");
            options.UseSqlServer(connectionString);
        });

        services.AddTransient<Func<string, BankLeafContext>>((provider) =>
        {
            return (leafCode) =>
            {
                var connectionString = configuration.GetConnectionString(leafCode);

                var options = new DbContextOptionsBuilder<BankLeafContext>()
                    .UseSqlServer(connectionString)
                    .Options;
                return new BankLeafContext(options);
            };
        });

        return services;
    }
}