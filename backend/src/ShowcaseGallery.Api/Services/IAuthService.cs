using ShowcaseGallery.Api.Models.DTOs;

namespace ShowcaseGallery.Api.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
    Task<UserProfileResponse?> GetUserProfileAsync(Guid userId);
}