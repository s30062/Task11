using Task11.Repository.interfaces;
using Task11.Services.dtos;
using Task11.Services.interfaces;

namespace Task11.Services.services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<GetAllEmployeeDto>> GetAllEmployees(CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllEmployees(cancellationToken);
        return employees.Select(e => new GetAllEmployeeDto
        {
            Id = e.Id,
            FullName = $"{e.Person.FirstName} {e.Person.MiddleName} {e.Person.LastName}".Replace("  ", " ").Trim()
        }).ToList();
    }

    public async Task<GetEmployeeById?> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeById(id, cancellationToken);
        if (employee == null) return null;

        return new GetEmployeeById
        {
            PersonDto = new PersonDto
            {
                Id = employee.Id,
                FirstName = employee.Person.FirstName,
                MiddleName = employee.Person.MiddleName,
                LastName = employee.Person.LastName,
                Email = employee.Person.Email,
                PassportNumber = employee.Person.PassportNumber,
                PhoneNumber = employee.Person.PhoneNumber
            },
            PositionDto = new PositionDto
            {
                Id = employee.Position.Id,
                PositionName = employee.Position.Name
            },
            HireDate = employee.HireDate,
            Salary = employee.Salary
        };
    }
}