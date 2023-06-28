using AutoMapper;
using DistributedTransactionsApi.Dtos;
using DistributedTransactionsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistributedTransactionsApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly DepartmentService _departmentService;
    private readonly IMapper _mapper;

    public DepartmentController(DepartmentService departmentService, IMapper mapper)
    {
        _departmentService = departmentService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        var departments = await _departmentService.GetDepartmentsAsync();

        return Ok(_mapper.Map<IEnumerable<DepartmentDto>>(departments));
    }
}