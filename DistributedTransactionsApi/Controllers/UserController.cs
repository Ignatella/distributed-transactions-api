using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistributedTransactionsApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public async void GetUser()
    {
        await Task.CompletedTask;
    }

    [HttpGet("Department")]
    public async void GetDepartment()
    {
        await Task.CompletedTask;
    }
    
    [HttpGet("Accounts")]
    public async void GetAccounts()
    {
        await Task.CompletedTask;
    }
    
    [HttpGet("Transactions")]
    public async void GetTransactions()
    {
        await Task.CompletedTask;
    }
    
    [HttpPost]
    public async void CreateUser()
    {
        await Task.CompletedTask;
    }
}