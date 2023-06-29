using System.Data;
using Dapper;
using DistributedTransactionsApi.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DistributedTransactionsApi.Services;

public class TransactionService
{
    private readonly UserUtility _userUtility;

    public TransactionService(UserUtility userUtility)
    {
        _userUtility = userUtility;
    }

    public async Task CreateTransactionAsync(string fromAccountNumber, string toAccountNumber, double amount,
        string description)
    {
        var userId = _userUtility.UserId;

        var context = await _userUtility.GetUserLeafContextAsync();

        await using var connection = context.Database.GetDbConnection();

        var param = new DynamicParameters();

        param.Add("@initiatorUserId", userId, DbType.Guid);
        param.Add("@fromAccountNumber", fromAccountNumber, DbType.String);
        param.Add("@toAccountNumber", toAccountNumber, DbType.String);
        param.Add("@amount", amount, DbType.Double);
        param.Add("@description", description, DbType.String);

        try
        {
            await connection.ExecuteAsync("uspCreateTransaction", param, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex) when (Enumerable.Range(51001, 3).Contains(ex.Number))
        {
            throw new ApplicationException(ex.Message);
        }
    }
}