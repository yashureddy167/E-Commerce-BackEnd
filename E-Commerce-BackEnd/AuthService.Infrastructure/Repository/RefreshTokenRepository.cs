using AuthService.Application.Interfaces.Repositories;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repository
{
    public class RefreshTokenRepository(AuthDbContext authDbContext): IRefreshTokenRepository
    {
        public async Task AddTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                await authDbContext.RefreshTokens.AddAsync(refreshToken);
                await authDbContext.SaveChangesAsync();
            }
            catch { throw; }
        }

        public async Task RemoveTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                authDbContext.RefreshTokens.Remove(refreshToken);
                await authDbContext.SaveChangesAsync();
            }
            catch { throw; }
        }

        public async Task<RefreshToken?> GetRefreshTokenDetails(string refreshToken)
        {
            try
            {
                var tokenDetails = await authDbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
                return tokenDetails;

            }
            catch { throw; }
        }

        public async Task UpdateTokenDetails(RefreshToken refreshToken)
        {
            try
            {
                authDbContext.RefreshTokens.Update(refreshToken);
                await authDbContext.SaveChangesAsync();
            }
            catch { throw; }
        }
    }
}
