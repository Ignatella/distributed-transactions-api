using System.Data;
using Dapper;
using DistributedTransactionsApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DistributedTransactionsApiTests.Core;

public class BaseFixture
{
    protected BankMasterContext MasterContext { get; }
    protected BankLeafContext LeafContext { get; }

    protected UserFactory UserFactory { get; }

    public BaseFixture()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var masterOptions = new DbContextOptionsBuilder<BankMasterContext>()
            .UseSqlServer(config.GetConnectionString("Master"))
            .Options;
        MasterContext = new BankMasterContext(masterOptions);

        var leafOptions = new DbContextOptionsBuilder<BankLeafContext>()
            .UseSqlServer(config.GetConnectionString("Leaf_1"))
            .Options;
        LeafContext = new BankLeafContext(leafOptions);

        UserFactory = new UserFactory(MasterContext, LeafContext);
    }

    [SetUp]
    public async Task SetupDatabase()
    {
        await UserFactory.GetAliceUserService().CreateUserAsync(UserFactory.GetAliceUser());
        await UserFactory.GetBobUserService().CreateUserAsync(UserFactory.GetBobUser());
    }

    [TearDown]
    public async Task CleanDatabase()
    {
        var connection = MasterContext.Database.GetDbConnection();

        await connection.ExecuteAsync("serviceRemoveUser", new { userId = UserFactory.AliceUserId },
            commandType: CommandType.StoredProcedure);

        await connection.ExecuteAsync("serviceRemoveUser", new { userId = UserFactory.BobUserId },
            commandType: CommandType.StoredProcedure);
    }
}