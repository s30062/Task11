namespace Task11.Services.interfaces;

public interface IJwtProvider
{
    string GenerateToken(int accountId, string username, string role);
}