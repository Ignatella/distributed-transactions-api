using DistributedTransactionsApi.Data;
using DistributedTransactionsApi.Data.Models.Master;

namespace DistributedTransactionsApi.Interfaces;

public interface IUserUtility
{
    Guid UserId { get; }
    Task<MasterUser> GetMasterUserAsync();
    Task<BankLeafContext> GetUserLeafContextAsync();
    Task<BankLeafContext> GetLeafContextAsync(Guid departmentId);
}