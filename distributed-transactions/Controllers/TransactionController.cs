using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace distributed_transactions.Controllers;

[Route("api/[controller]")]
[Authorize]
public class TransactionController: ControllerBase
{
    [HttpPost("In")]
    public async void In()
    {
        await Task.CompletedTask;
    }
    
    [HttpPost("Out")]
    public async void Out()
    {
        await Task.CompletedTask;
    }
    
    [HttpPost("To/{userId:guid}")]
    public async void To(Guid userId)
    {
        await Task.CompletedTask;
    }
}