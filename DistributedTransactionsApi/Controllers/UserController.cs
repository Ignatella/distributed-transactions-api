using AutoMapper;
using DistributedTransactionsApi.Data;
using DistributedTransactionsApi.Data.Models.Leaf;
using DistributedTransactionsApi.Dtos;
using DistributedTransactionsApi.Interfaces;
using DistributedTransactionsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistributedTransactionsApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly BankMasterContext _masterContext;
    private readonly UserService _userService;
    private readonly IUserUtility _userUtility;
    private readonly IMapper _mapper;

    public UserController(
        BankMasterContext masterContext,
        IUserUtility userUtility,
        UserService userService,
        IMapper mapper)
    {
        _masterContext = masterContext;
        _userUtility = userUtility;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        var user = await _userUtility.GetMasterUserAsync();

        if (user is null) return NotFound();

        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpGet("Department")]
    public async Task<IActionResult> GetUserDepartment()
    {
        var department = await _userService.GetUserDepartmentAsync();

        if (department is null) return NotFound();

        return Ok(_mapper.Map<DepartmentDto>(department));
    }

    [HttpGet("Accounts")]
    public async Task<IActionResult> GetAccounts()
    {
        var user = await _userUtility.GetMasterUserAsync();

        if (user is null) return NotFound();

        var accounts = await _userService.GetUserAccountsAsync();

        return Ok(_mapper.Map<IEnumerable<AccountDto>>(accounts));
    }

    [HttpGet("Transactions")]
    public async Task<IActionResult> GetTransactions()
    {
        var user = await _userUtility.GetMasterUserAsync();

        if (user is null) return NotFound();

        var transactions = await _userService.GetUserTransactionsAsync();

        return Ok(_mapper.Map<IEnumerable<TransactionDto>>(transactions));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
    {
        var user = _mapper.Map<LeafUser>(userCreateDto,
            opt => opt.AfterMap((_, dest) => dest.UserId = _userUtility.UserId));

        await _userService.CreateUserAsync(user);

        return NoContent();
    }
}