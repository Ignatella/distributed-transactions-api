using System.ComponentModel.DataAnnotations;
using DistributedTransactionsApi.Attributes;

namespace DistributedTransactionsApi.Dtos;

public class TransactionDto
{
    public Guid TransactionId { get; set; }

    public Guid? FromAccountId { get; set; }

    public Guid? ToAccountId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }
}

public class TransactionCreateDto
{
    [Required]
    [StringLength(26, MinimumLength = 26)]
    public string AccountNumber { get; set; }

    [Required] [GreaterThanZero] public double Amount { get; set; }

    public string Description { get; set; } = null;
}