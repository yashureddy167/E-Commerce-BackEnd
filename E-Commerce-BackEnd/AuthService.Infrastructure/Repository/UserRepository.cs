using AuthService.Application.Interfaces.Repositories;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repository
{
    public class UserRepository(AuthDbContext db): IUserRepository
    {
        public async Task<bool> AddUserAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                await db.Users.AddAsync(user, cancellationToken);
                var result = await db.SaveChangesAsync(cancellationToken);
                return result > 0;
            }
            catch { throw; }
        }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                return await db.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            }
            catch { throw; }
        }

        public async Task<User?> GetUserByUserIdAsync(long userId, CancellationToken cancellationToken)
        {
            try
            {
                return await db.Users.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
            }
            catch { throw; }
        }
    }
}
