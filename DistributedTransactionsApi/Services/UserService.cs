using DistributedTransactionsApi.Data;
using DistributedTransactionsApi.Data.Models.Leaf;
using DistributedTransactionsApi.Data.Models.Master;
using DistributedTransactionsApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DistributedTransactionsApi.Services;

public class UserService
{
    private readonly BankMasterContext _masterContext;
    private readonly UserUtility _userUtility;

    public UserService(BankMasterContext masterContext, UserUtility userUtility)
    {
        _masterContext = masterContext;
        _userUtility = userUtility;
    }

    public async Task<Department> GetUserDepartmentAsync(Guid userId)
    {
        return await _masterContext
            .MasterUsers
            .Where(u => u.UserId == userId)
            .Select(u => u.Department)
            .FirstOrDefaultAsync();
    }

    public async Task CreateUserAsync(LeafUser user)
    {
        var context = await _userUtility.GetUserLeafContextAsync(user.DepartmentId);

        context.LeafUsers.Add(user);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}