

using System.ComponentModel.DataAnnotations;
namespace Task11.Services.dtos;


public class RegisterAccountDto
{
    [Required]
    [RegularExpression("^[^0-9].*", ErrorMessage = "username must not start with a number")]
    [StringLength(100, MinimumLength = 3)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 12, ErrorMessage = "password must be at least 12 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "password must contain at least one lowercase letter, one uppercase letter, one digit, and one symbol")]
    public string Password { get; set; } = null!;

    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public string Role { get; set; } = null!;
}
