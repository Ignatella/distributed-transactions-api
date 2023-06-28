namespace DistributedTransactionsApi.Dtos;

public class AccountDto
{
    public Guid AccountId { get; set; }

    public Guid AccountTypeId { get; set; }

    public string AccountNumber { get; set; }

    public decimal Balance { get; set; }

    public Guid UserId { get; set; }

    public AccountTypeDto AccountType { get; set; }
}