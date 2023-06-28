namespace DistributedTransactionsApi.Dtos;

public class TransactionDto
{
    public Guid TransactionId { get; set; }

    public Guid? FromAccountId { get; set; }

    public Guid? ToAccountId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }
}