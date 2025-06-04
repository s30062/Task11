using Task11.Services.dtos;

namespace Task11.Services.interfaces;

public interface IEmployeeService
{
    Task<List<GetAllEmployeeDto>> GetAllEmployees(CancellationToken cancellationToken);
    Task<GetEmployeeById?> GetEmployeeById(int id, CancellationToken cancellationToken);
}