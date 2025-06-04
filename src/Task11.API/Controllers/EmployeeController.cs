using Task11.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Task11.API.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize(Roles = "Admin")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var employees = await _employeeService.GetAllEmployees(cancellationToken);
        return employees.Any() ? Ok(employees) : NotFound("No employees found.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var employee = await _employeeService.GetEmployeeById(id, cancellationToken);
        return employee != null ? Ok(employee) : NotFound("Employee not found.");
    }
}