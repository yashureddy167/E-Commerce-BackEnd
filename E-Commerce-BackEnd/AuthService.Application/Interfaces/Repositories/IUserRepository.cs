using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetUserByEmailAsync(string username, CancellationToken cancellationToken);
        Task<User?> GetUserByUserIdAsync(long userId, CancellationToken cancellationToken);
    }
}
