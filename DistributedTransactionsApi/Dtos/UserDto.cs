using System.ComponentModel.DataAnnotations;

namespace DistributedTransactionsApi.Dtos;

public class UserDto
{
    public Guid UserId { get; set; }

    public Guid DepartmentId { get; set; }
}

public class UserCreateDto
{
    [Required] [MaxLength(20)] public string PhoneNumber { get; set; }

    [Required] public Guid DepartmentId { get; set; }

    [Required] public AddressCreateDto Address { get; set; }
}