namespace AuthService.Application.Data
{
    public class RefreshTokenData
    {
        public required string Token { get; init; }
        public DateTime ExpiresAt { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public long UserId { get; init; }
    }
}
