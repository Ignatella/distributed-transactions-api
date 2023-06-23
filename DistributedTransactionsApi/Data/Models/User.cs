using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public Guid DepartmentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Department Department { get; set; }

    public virtual ICollection<Transaction> TransactionFromUsers { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionToUsers { get; set; } = new List<Transaction>();
}
