using Task11.Services.dtos;

namespace EmployeeManager.Services.interfaces;

public interface IAccountService
{
    Task RegisterAsync(RegisterAccountDto dto, CancellationToken cancellationToken);
    Task<string> AuthenticateAsync(LoginDto dto, CancellationToken cancellationToken);
}