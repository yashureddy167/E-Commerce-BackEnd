using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace AuthService.Domain.Entities
{
    public record RefreshToken
    {
        public long RefreshTokenId { get; set; }
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public bool IsActive => RevokedAt == null && DateTime.UtcNow <= ExpiresAt;
        [ForeignKey("User")]
        public long UserId { get; set; }
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;

        // Navigation property
        public User User { get; set; } = null!;
    }
}
