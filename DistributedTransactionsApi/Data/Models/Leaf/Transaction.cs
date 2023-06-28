using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models.Leaf;

public partial class Transaction
{
    public Guid TransactionId { get; set; }

    public Guid? FromAccountId { get; set; }

    public Guid? ToAccountId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }
}
