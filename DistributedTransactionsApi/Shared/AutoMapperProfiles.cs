using AutoMapper;
using DistributedTransactionsApi.Data.Models.Leaf;
using DistributedTransactionsApi.Data.Models.Master;
using DistributedTransactionsApi.Dtos;

namespace DistributedTransactionsApi.Shared;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Address, AddressDto>();
        CreateMap<AddressCreateDto, Address>();

        CreateMap<Department, DepartmentDto>();

        CreateMap<MasterUser, UserDto>();
        CreateMap<LeafUser, UserDto>();
        CreateMap<UserCreateDto, LeafUser>();
    }
}