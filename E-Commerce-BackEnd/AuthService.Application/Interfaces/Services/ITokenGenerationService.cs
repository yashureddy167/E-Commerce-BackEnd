using AuthService.Domain.Data;
using System.Numerics;

namespace AuthService.Application.Interfaces.Services
{
    public interface ITokenGenerationService
    {
        Task<TokensData> GenerateAccessAndRefreshTokensAsync(long userId, string email, string role);
    }
}
