using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models.Leaf;

public partial class Address
{
    public Guid AddressId { get; set; }

    public string PostalCode { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public string FlatNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<LeafUser> LeafUsers { get; } = new List<LeafUser>();
}
