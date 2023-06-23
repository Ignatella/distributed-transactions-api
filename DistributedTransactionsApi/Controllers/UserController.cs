using AutoMapper;
using DistributedTransactionsApi.Data;
using DistributedTransactionsApi.Data.Models.Leaf;
using DistributedTransactionsApi.Dtos;
using DistributedTransactionsApi.Services;
using DistributedTransactionsApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistributedTransactionsApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly BankMasterContext _masterContext;
    private readonly UserUtility _userUtility;
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public UserController(
        BankMasterContext masterContext,
        UserUtility userUtility,
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
        var department = await _userService.GetUserDepartmentAsync(_userUtility.UserId);

        if (department is null) return NotFound();

        return Ok(_mapper.Map<DepartmentDto>(department));
    }

    [HttpGet("Accounts")]
    public async void GetAccounts()
    {
        await Task.CompletedTask;
    }

    [HttpGet("Transactions")]
    public async void GetTransactions()
    {
        await Task.CompletedTask;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
    {
        var user = _mapper.Map<LeafUser>(userCreateDto);
        // ToDO: pass as map parameter
        user.UserId = _userUtility.UserId;

        await _userService.CreateUserAsync(user);

        return NoContent();
    }
}