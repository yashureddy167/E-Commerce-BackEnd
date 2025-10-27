using System.Numerics;

namespace AuthService.Domain.Data
{
    public class RefreshTokenData
    {
        public required string Token { get; init; }
        public DateTime ExpiresAt { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public BigInteger UserId { get; init; }
    }
}
