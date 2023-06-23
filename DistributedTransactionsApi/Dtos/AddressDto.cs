using System.ComponentModel.DataAnnotations;

namespace DistributedTransactionsApi.Dtos;

public class AddressDto
{
    public Guid AddressId { get; set; }

    public string PostalCode { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public string FlatNumber { get; set; }
}

public class AddressCreateDto
{
    [Required] [MaxLength(10)] public string PostalCode { get; set; }

    [Required] [MaxLength(100)] public string City { get; set; }

    [Required] [MaxLength(100)] public string Street { get; set; }

    [Required] [MaxLength(10)] public string HouseNumber { get; set; }

    [MaxLength(10)] public string FlatNumber { get; set; } = null;
}