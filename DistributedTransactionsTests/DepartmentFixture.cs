using DistributedTransactionsApi.Services;
using DistributedTransactionsApiTests.Core;
using NUnit.Framework;

namespace DistributedTransactionsApiTests;

[TestFixture]
public class DepartmentFixture : BaseFixture
{
    private readonly DepartmentService _departmentService;

    public DepartmentFixture()
    {
        _departmentService = new DepartmentService(MasterContext);
    }

    [Test]
    public async Task DepartmentPresentedInMasterDatabase()
    {
        var bankDepartments = await _departmentService.GetDepartmentsAsync();
        var departments = bankDepartments.ToList();

        Assert.That(departments, Is.Not.Null);
        Assert.That(departments.Count, Is.EqualTo(4));
    }
}