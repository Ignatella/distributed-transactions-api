using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models.Leaf;

public partial class LeafUser
{
    public Guid UserId { get; set; }

    public string PhoneNumber { get; set; }

    public Guid AddressId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid DepartmentId { get; set; }

    public virtual Address Address { get; set; }
}
