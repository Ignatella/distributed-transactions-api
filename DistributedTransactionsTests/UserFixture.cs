using DistributedTransactionsApiTests.Core;
using NUnit.Framework;

namespace DistributedTransactionsApiTests;

[TestFixture]
public class UserFixture : BaseFixture
{
    [Test]
    public async Task UserPresentedInMasterDatabase()
    {
        var user = await MasterContext.MasterUsers.FindAsync(UserFactory.AliceUserId);

        Assert.That(user, Is.Not.Null);
        Assert.That(user!.UserId, Is.EqualTo(UserFactory.AliceUserId));
    }

    [Test]
    public async Task UserPresentedInLeafDatabase()
    {
        var user = await LeafContext.LeafUsers.FindAsync(UserFactory.AliceUserId);

        Assert.That(user, Is.Not.Null);
        Assert.That(user!.UserId, Is.EqualTo(UserFactory.AliceUserId));
    }

    [Test]
    public async Task UserHas2Accounts()
    {
        var userAccounts = await UserFactory.GetAliceUserService().GetUserAccountsAsync();
        var accounts = userAccounts.ToList();

        Assert.That(accounts, Is.Not.Null);
        Assert.That(accounts, Is.Not.Empty);
        Assert.That(accounts.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task UserHasNoTransactions()
    {
        var userTransactions = await UserFactory.GetAliceUserService().GetUserTransactionsAsync();
        var transactions = userTransactions.ToList();

        Assert.That(transactions, Is.Not.Null);
        Assert.That(transactions, Is.Empty);
    }

    [Test]
    public async Task UserHasAssignedDepartment()
    {
        var userDepartment = await UserFactory.GetAliceUserService().GetUserDepartmentAsync();

        Assert.That(userDepartment, Is.Not.Null);
        Assert.That(userDepartment.DepartmentId, Is.EqualTo(UserFactory.AliceDepartmentId));
    }
}