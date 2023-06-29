using DistributedTransactionsApi.Dtos;
using DistributedTransactionsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistributedTransactionsApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("In")]
    public async Task<IActionResult> In([FromBody] TransactionCreateDto transaction)
    {
        await _transactionService.CreateTransactionAsync(null, transaction.AccountNumber, transaction.Amount,
            transaction.Description);

        return NoContent();
    }

    [HttpPost("Out")]
    public async Task<IActionResult> Out([FromBody] TransactionCreateDto transaction)
    {
        await _transactionService.CreateTransactionAsync(transaction.AccountNumber, null, transaction.Amount,
            transaction.Description);

        return NoContent();
    }

    [HttpPost("To/{accountNumber}")]
    public async Task<IActionResult> To(string accountNumber, [FromBody] TransactionCreateDto transaction)
    {
        await _transactionService.CreateTransactionAsync(transaction.AccountNumber, accountNumber, transaction.Amount,
            transaction.Description);

        return NoContent();
    }
}