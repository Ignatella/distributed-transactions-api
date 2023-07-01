using System.Data;
using Dapper;
using DistributedTransactionsApi.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DistributedTransactionsApi.Services;

public class TransactionService
{
    private readonly IUserUtility _userUtility;

    public TransactionService(IUserUtility userUtility)
    {
        _userUtility = userUtility;
    }

    public async Task CreateTransactionAsync(string fromAccountNumber, string toAccountNumber, double amount,
        string description)
    {
        var userId = _userUtility.UserId;

        var context = await _userUtility.GetUserLeafContextAsync();

        var connection = context.Database.GetDbConnection();

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
        catch (SqlException ex) when (ex.Number is > 51000 and < 52000)
        {
            throw new ApplicationException(ex.Message);
        }
    }
}