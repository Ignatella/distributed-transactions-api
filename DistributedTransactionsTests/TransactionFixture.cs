using DistributedTransactionsApi.Data.Models.Master;
using DistributedTransactionsApi.Services;
using DistributedTransactionsApiTests.Core;
using NUnit.Framework;

namespace DistributedTransactionsApiTests;

[TestFixture]
public class TransactionFixture : BaseFixture
{
    private readonly TransactionService _aliceTransactionService;
    private readonly TransactionService _bobTransactionService;

    private IEnumerable<Account>? _aliceAccounts;
    private IEnumerable<Account>? _bobAccounts;

    private Account? _aliceAccount;
    private Account? _bobAccount;

    public TransactionFixture()
    {
        _aliceTransactionService = new TransactionService(UserFactory.GetAliceUserUtility());
        _bobTransactionService = new TransactionService(UserFactory.GetBobUserUtility());
    }

    private async Task RefreshUserAccounts()
    {
        _aliceAccounts = await UserFactory.GetAliceUserService().GetUserAccountsAsync();
        _bobAccounts = await UserFactory.GetBobUserService().GetUserAccountsAsync();

        _aliceAccount = _aliceAccounts.First();
        _bobAccount = _bobAccounts.First();
    }

    [SetUp]
    public async Task Setup()
    {
        await RefreshUserAccounts();
    }

    [Test]
    public async Task CanCreateDepositTransaction()
    {
        await _aliceTransactionService.CreateTransactionAsync(null, _aliceAccount!.AccountNumber, 100,
            "Test deposit transaction");

        var aliceTransactions = await UserFactory.GetAliceUserService().GetUserTransactionsAsync();
        var transactions = aliceTransactions.ToList();

        Assert.That(transactions, Is.Not.Null);
        Assert.That(transactions.Count, Is.EqualTo(1));
        Assert.That(transactions.First().Amount, Is.EqualTo(100));
        Assert.That(transactions.First().FromAccountId, Is.Null);
        Assert.That(transactions.First().ToAccountId, Is.EqualTo(_aliceAccount.AccountId));
    }

    [Test]
    public async Task CanCreateWithdrawTransaction()
    {
        await _aliceTransactionService.CreateTransactionAsync(null, _aliceAccount!.AccountNumber, 100,
            "Test deposit transaction");

        await _aliceTransactionService.CreateTransactionAsync(_aliceAccount.AccountNumber, null, 50,
            "Test withdraw transaction");

        var aliceTransactions = await UserFactory.GetAliceUserService().GetUserTransactionsAsync();
        var transactions = aliceTransactions.ToList();

        await RefreshUserAccounts();

        Assert.That(transactions, Is.Not.Null);
        Assert.That(transactions.Count, Is.EqualTo(2));
        Assert.That(transactions.First().Amount, Is.EqualTo(50));
        Assert.That(transactions.Last().Amount, Is.EqualTo(100));
        Assert.That(transactions.First().ToAccountId, Is.Null);
        Assert.That(transactions.Last().FromAccountId, Is.Null);
        Assert.That(transactions.First().FromAccountId, Is.EqualTo(_aliceAccount.AccountId));
        Assert.That(transactions.Last().ToAccountId, Is.EqualTo(_aliceAccount.AccountId));
        Assert.That(_aliceAccount.Balance, Is.EqualTo(50));
    }

    [Test]
    public async Task CanCreateTransaction()
    {
        await _aliceTransactionService.CreateTransactionAsync(null, _aliceAccount!.AccountNumber, 100,
            "Test deposit transaction");

        await _aliceTransactionService.CreateTransactionAsync(_aliceAccount!.AccountNumber, _bobAccount!.AccountNumber,
            50, "Test deposit transaction");

        var aliceTransactions = await UserFactory.GetAliceUserService().GetUserTransactionsAsync();
        var bobTransactions = await UserFactory.GetBobUserService().GetUserTransactionsAsync();

        var aliceTransactionsList = aliceTransactions.ToList();
        var bobTransactionsList = bobTransactions.ToList();

        await RefreshUserAccounts();

        Assert.That(aliceTransactionsList, Is.Not.Null);
        Assert.That(aliceTransactionsList.Count, Is.EqualTo(2));
        Assert.That(bobTransactionsList.Count, Is.EqualTo(1));
        Assert.That(aliceTransactionsList.First().Amount, Is.EqualTo(50));
        Assert.That(aliceTransactionsList.Last().Amount, Is.EqualTo(100));

        Assert.That(_aliceAccount!.Balance, Is.EqualTo(50));
        Assert.That(_bobAccount!.Balance, Is.EqualTo(50));
    }

    [Test]
    public async Task CanCreateHomeTransaction()
    {
        await _aliceTransactionService.CreateTransactionAsync(null, _aliceAccount!.AccountNumber, 100,
            "Test deposit transaction");

        await _aliceTransactionService.CreateTransactionAsync(_aliceAccount!.AccountNumber,
            _aliceAccounts!.Last().AccountNumber,
            50, "Test home transaction");

        var aliceTransactions = await UserFactory.GetAliceUserService().GetUserTransactionsAsync();

        var aliceTransactionsList = aliceTransactions.ToList();

        await RefreshUserAccounts();

        Assert.That(aliceTransactionsList, Is.Not.Null);
        Assert.That(aliceTransactionsList.Count, Is.EqualTo(2));
        Assert.That(aliceTransactionsList.First().Amount, Is.EqualTo(50));
        Assert.That(aliceTransactionsList.Last().Amount, Is.EqualTo(100));
        Assert.That(_aliceAccounts!.First().Balance, Is.EqualTo(50));
        Assert.That(_aliceAccounts!.Last().Balance, Is.EqualTo(50));
    }

    [Test]
    public async Task CantWithdrawTooMuchMoney()
    {
        await _aliceTransactionService.CreateTransactionAsync(null, _aliceAccount!.AccountNumber, 100,
            "Test deposit transaction");

        Assert.ThrowsAsync<ApplicationException>(async () => await _aliceTransactionService.CreateTransactionAsync(
            _aliceAccount!.AccountNumber, null,
            500, "Test deposit transaction"));
    }

    [Test]
    public async Task CantDepositOnAnotherUserAccount()
    {
        await _aliceTransactionService.CreateTransactionAsync(null, _aliceAccount!.AccountNumber, 100,
            "Test deposit transaction");

        Assert.ThrowsAsync<ApplicationException>(async () => await _aliceTransactionService.CreateTransactionAsync(
            null, _bobAccount!.AccountNumber,
            500, "Test deposit transaction"));
    }


    [Test]
    public async Task CantTransferToTheSameAccount()
    {
        await _aliceTransactionService.CreateTransactionAsync(null, _aliceAccount!.AccountNumber, 100,
            "Test deposit transaction");

        Assert.ThrowsAsync<ApplicationException>(async () => await _aliceTransactionService.CreateTransactionAsync(
            _aliceAccount!.AccountNumber, _aliceAccount!.AccountNumber,
            500, "Test deposit transaction"));
    }
}