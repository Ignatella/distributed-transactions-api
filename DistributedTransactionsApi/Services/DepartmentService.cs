using DistributedTransactionsApi.Data;
using DistributedTransactionsApi.Data.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace DistributedTransactionsApi.Services;

public class DepartmentService
{
    private readonly BankMasterContext _masterContext;

    public DepartmentService(BankMasterContext masterContext)
    {
        _masterContext = masterContext;
    }

    public async Task<IEnumerable<Department>> GetDepartmentsAsync()
    {
        return await _masterContext.Departments.ToListAsync();
    }
}