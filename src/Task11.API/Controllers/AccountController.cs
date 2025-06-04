using Task11.API;
using Task11.Services.dtos;
using Task11.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Task11.API.Controllers;

[ApiController]
[Route("api")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly EmployeeDatabaseContext _context;

    public AccountController(IAccountService accountService, EmployeeDatabaseContext context)
    {
        _accountService = accountService;
        _context = context;
    }

    [HttpPost("accounts")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register(RegisterAccountDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _accountService.RegisterAsync(dto, cancellationToken);
            return StatusCode(201);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("auth")]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate(LoginDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var token = await _accountService.AuthenticateAsync(dto, cancellationToken);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid username or password.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("accounts/me")]
    [Authorize]
    public async Task<IActionResult> GetOwnAccount(CancellationToken cancellationToken)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var account = await _context.Accounts
                .Include(a => a.Role)
                .Include(a => a.Employee)
                .ThenInclude(e => e.Person)
                .FirstOrDefaultAsync(a => a.Id == userId, cancellationToken);

            if (account == null) return NotFound();

            var person = account.Employee.Person;

            return Ok(new
            {
                account.Username,
                Role = account.Role.Name,
                EmployeeId = account.EmployeeId,
                Person = new
                {
                    person.FirstName,
                    person.MiddleName,
                    person.LastName,
                    person.Email,
                    person.PhoneNumber
                }
            });
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPut("accounts/me")]
    [Authorize]
    public async Task<IActionResult> UpdateOwnAccount(UpdateOwnInfoDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var account = await _context.Accounts
                .Include(a => a.Employee)
                .ThenInclude(e => e.Person)
                .FirstOrDefaultAsync(a => a.Id == userId, cancellationToken);

            if (account == null) return NotFound("Account not found.");

            var person = account.Employee.Person;
            person.FirstName = dto.FirstName;
            person.MiddleName = dto.MiddleName;
            person.LastName = dto.LastName;
            person.PhoneNumber = dto.PhoneNumber;
            person.Email = dto.Email;

            await _context.SaveChangesAsync(cancellationToken);
            return Ok("Profile updated successfully.");
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"Update failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
