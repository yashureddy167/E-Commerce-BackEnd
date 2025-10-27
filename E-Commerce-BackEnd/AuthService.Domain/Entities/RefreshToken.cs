using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace AuthService.Domain.Entities
{
    public record RefreshToken
    {
        public BigInteger RefreshTokenId { get; init; }
        public required string Token { get; init; }
        public DateTime ExpiresAt { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? RevokedAt { get; init; }
        public bool IsActive => RevokedAt == null && DateTime.UtcNow <= ExpiresAt;
        [ForeignKey("User")]
        public BigInteger UserId { get; init; }
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;

        // Navigation property
        public User User { get; init; } = null!;
    }
}
