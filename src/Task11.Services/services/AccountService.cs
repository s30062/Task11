namespace DefaultNamespace;

using Task11.API;
using Task11.Repository.context;
using Task11.Services.dtos;
using Task11.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Task11.Services.services;

public class AccountService : IAccountService
{
    private readonly EmployeeDatabaseContext _context;
    private readonly IJwtProvider _jwtProvider;

    public AccountService(EmployeeDatabaseContext context, IJwtProvider jwtProvider)
    {
        _context = context;
        _jwtProvider = jwtProvider;
    }

    public async Task RegisterAsync(RegisterAccountDto dto, CancellationToken cancellationToken)
    {
        var exists = await _context.Accounts.AnyAsync(a => a.Username == dto.Username, cancellationToken);
        if (exists) throw new ArgumentException("Username already taken.");

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role, cancellationToken);
        if (role == null) throw new ArgumentException("Invalid role.");

        var employee = await _context.Employees.FindAsync(new object[] { dto.EmployeeId }, cancellationToken);
        if (employee == null) throw new ArgumentException("Employee not found.");

        PasswordHasher.CreatePasswordHash(dto.Password, out var hash, out var salt);

        var account = new Account
        {
            Username = dto.Username,
            PasswordHash = hash,
            PasswordSalt = salt,
            RoleId = role.Id,
            EmployeeId = employee.Id
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> AuthenticateAsync(LoginDto dto, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Username == dto.Username, cancellationToken);

        if (account == null || !PasswordHasher.VerifyPassword(dto.Password, account.PasswordHash, account.PasswordSalt))
            throw new UnauthorizedAccessException("Invalid credentials.");

        return _jwtProvider.GenerateToken(account.Id, account.Username, account.Role.Name);
    }
}
