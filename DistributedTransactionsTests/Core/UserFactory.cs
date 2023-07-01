using DistributedTransactionsApi.Data;
using DistributedTransactionsApi.Data.Models.Leaf;
using DistributedTransactionsApi.Interfaces;
using DistributedTransactionsApi.Services;
using Moq;

namespace DistributedTransactionsApiTests.Core;

public class UserFactory
{
    private readonly BankMasterContext _masterContext;
    private readonly BankLeafContext _leafContext;
    public Guid AliceUserId { get; } = Guid.NewGuid();
    public Guid BobUserId { get; } = Guid.NewGuid();

    public Guid AliceDepartmentId { get; } = Guid.Parse("272EEBCE-A2C2-4888-AC74-1844A8F94CCA"); // Leaf_1
    public Guid BobDepartmentId { get; } = Guid.Parse("92003B30-30A0-4ADE-B912-CEBDEFD98F49"); // Leaf_2

    public UserFactory(BankMasterContext masterContext, BankLeafContext leafContext)
    {
        _masterContext = masterContext;
        _leafContext = leafContext;
    }

    public LeafUser GetAliceUser()
    {
        return new LeafUser()
        {
            UserId = AliceUserId,
            DepartmentId = AliceDepartmentId,
            PhoneNumber = "123456789",
            Address = new Address()
            {
                PostalCode = "12-345",
                City = "Krakow",
                Street = "Budryka",
                HouseNumber = "1",
                FlatNumber = "2"
            }
        };
    }

    public LeafUser GetBobUser()
    {
        return new LeafUser()
        {
            UserId = BobUserId,
            DepartmentId = BobDepartmentId,
            PhoneNumber = "987654321",
            Address = new Address()
            {
                PostalCode = "54-321",
                City = "Warsaw",
                Street = "Krucza",
                HouseNumber = "3",
                FlatNumber = "4"
            }
        };
    }

    public IUserUtility GetAliceUserUtility()
    {
        var userUtilityMock = new Mock<IUserUtility>();
        userUtilityMock.SetupGet(s => s.UserId).Returns(AliceUserId);
        userUtilityMock.Setup(s => s.GetUserLeafContextAsync()).Returns(Task.FromResult(_leafContext));
        userUtilityMock.Setup(s => s.GetLeafContextAsync(AliceDepartmentId)).Returns(Task.FromResult(_leafContext));
        return userUtilityMock.Object;
    }

    public IUserUtility GetBobUserUtility()
    {
        var userUtilityMock = new Mock<IUserUtility>();
        userUtilityMock.SetupGet(s => s.UserId).Returns(BobUserId);
        userUtilityMock.Setup(s => s.GetUserLeafContextAsync()).Returns(Task.FromResult(_leafContext));
        userUtilityMock.Setup(s => s.GetLeafContextAsync(BobDepartmentId)).Returns(Task.FromResult(_leafContext));
        return userUtilityMock.Object;
    }

    public UserService GetAliceUserService()
    {
        return new UserService(_masterContext, GetAliceUserUtility());
    }

    public UserService GetBobUserService()
    {
        return new UserService(_masterContext, GetBobUserUtility());
    }
}