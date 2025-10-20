namespace AuthService.Application.Interfaces.Services
{
    public interface ITokenGenerationService
    {
        Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokensAsync(long userId, string email, string role);
    }
}
