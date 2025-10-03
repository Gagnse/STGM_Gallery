using System.Security.Claims;

namespace ShowcaseGallery.Api.Services;

public interface IJwtService
{
    string GenerateAccessToken(Guid userId, string email, string username);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token);
}