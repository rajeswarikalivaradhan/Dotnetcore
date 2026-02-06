using WebApi.DTOs.Auth;
using WebApi.Models;

namespace WebApi.Services;

public interface IAuthService
{
    Task<LoginResponse> RegisterAsync(RegisterRequest request);
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task ForgotPasswordAsync(ForgotPasswordRequest request);
    Task ChangePasswordAsync(int userId, ChangePasswordRequest request);
    string GenerateJwtToken(User user);
}
