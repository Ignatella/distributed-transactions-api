using System.Security.Claims;
using DistributedTransactionsApi.Data;
using DistributedTransactionsApi.Data.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace DistributedTransactionsApi.Utilities;

public class UserUtility
{
    private readonly BankMasterContext _masterContext;
    private readonly Func<string, BankLeafContext> _leafContextFactory;

    public Guid UserId { get; }

    public UserUtility(IHttpContextAccessor httpContextAccessor, BankMasterContext masterContext,
        Func<string, BankLeafContext> leafContextFactory)
    {
        _masterContext = masterContext;
        _leafContextFactory = leafContextFactory;

        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) throw new ApplicationException("User not found");

        UserId = Guid.Parse(userId);
    }

    public async Task<MasterUser> GetMasterUserAsync()
    {
        return await _masterContext.MasterUsers.Include(u => u.Department).FirstOrDefaultAsync(u => u.UserId == UserId);
    }

    public async Task<BankLeafContext> GetUserLeafContextAsync()
    {
        var user = await GetMasterUserAsync();
        if (user == null) throw new ApplicationException("User not found");

        var departmentCode = user.Department.Code;

        return _leafContextFactory(departmentCode);
    }

    public async Task<BankLeafContext> GetLeafContextAsync(Guid departmentId)
    {
        var department = await _masterContext.Departments.FindAsync(departmentId);
        if (department == null) throw new ApplicationException("Department not found");

        return _leafContextFactory(department.Code);
    }
}