using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities
{
    public record User
    {
        [Key]
        public long UserId { get; init; }
        [Required]
        [StringLength(20)]
        public required string FirstName { get; init; }
        [Required]
        [StringLength(20)]
        public required string LastName { get; init; }
        [Required]
        [EmailAddress]
        public required string Email { get; init; }
        [Required]
        public required string PasswordHash { get; init; }
        [Required]
        public DateTime DateOfBirth { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? LastUpdatedAt { get; init; }
        public bool IsTwoFactorAuthEnabled { get; init; } = false;
        public string Role { get; init; } = "User";

        public ICollection<RefreshToken> RefreshTokens { get; init; } = new List<RefreshToken>();
    }
}
