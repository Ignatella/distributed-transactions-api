using System.Data;
using Dapper;
using DistributedTransactionsApi.Data;
using DistributedTransactionsApi.Data.Models.Leaf;
using DistributedTransactionsApi.Data.Models.Master;
using DistributedTransactionsApi.Interfaces;
using DistributedTransactionsApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DistributedTransactionsApi.Services;

public class UserService
{
    private readonly BankMasterContext _masterContext;
    private readonly IUserUtility _userUtility;

    public UserService(BankMasterContext masterContext, IUserUtility userUtility)
    {
        _masterContext = masterContext;
        _userUtility = userUtility;
    }

    public async Task<Department> GetUserDepartmentAsync()
    {
        var userId = _userUtility.UserId;

        return await _masterContext
            .MasterUsers
            .Where(u => u.UserId == userId)
            .Select(u => u.Department)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Account>> GetUserAccountsAsync()
    {
        var userId = _userUtility.UserId;

        var context = await _userUtility.GetUserLeafContextAsync();

        var connection = context.Database.GetDbConnection();

        var param = new
        {
            userId
        };

        var accounts = await connection.QueryAsync<Account, AccountType, Account>("uspGetUserAccounts",
            (account, accountType) =>
            {
                account.AccountType = accountType;
                return account;
            }, param: param, commandType: CommandType.StoredProcedure, splitOn: nameof(AccountType.AccountTypeId));

        return accounts;
    }

    public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync()
    {
        var userId = _userUtility.UserId;

        var context = await _userUtility.GetUserLeafContextAsync();

        var connection = context.Database.GetDbConnection();

        var param = new
        {
            userId
        };

        var transactions = await connection.QueryAsync<Transaction>("uspGetUserTransactions", param,
            commandType: CommandType.StoredProcedure);

        return transactions;
    }

    public async Task CreateUserAsync(LeafUser user)
    {
        var context = await _userUtility.GetLeafContextAsync(user.DepartmentId);

        var connection = context.Database.GetDbConnection();

        var userParameters = new
        {
            user.UserId,
            user.PhoneNumber,
            user.Address.PostalCode,
            user.Address.City,
            user.Address.Street,
            user.Address.HouseNumber,
            user.Address.FlatNumber
        };

        await connection.ExecuteAsync("uspUserCreate", userParameters, commandType: CommandType.StoredProcedure);
    }
}