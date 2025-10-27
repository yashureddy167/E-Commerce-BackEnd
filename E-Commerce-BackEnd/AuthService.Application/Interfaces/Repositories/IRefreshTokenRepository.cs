using AuthService.Domain.Entities;
namespace AuthService.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddTokenAsync(RefreshToken refreshToken);
        Task RemoveTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenDetails(string refreshToken);
        Task UpdateTokenDetails(RefreshToken refreshToken);
    }
}
