using AuthService.Application.Data;

namespace AuthService.Application.Interfaces.Services
{
    public interface ITokenGenerationService
    {
        Task<TokensData> GenerateAccessAndRefreshTokensAsync(long userId, string email, string role);
    }
}
