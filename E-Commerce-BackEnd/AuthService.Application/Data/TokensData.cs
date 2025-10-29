namespace AuthService.Application.Data
{
    public record TokensData
    {
        public string AccessToken { get; init; } = null!;
        public RefreshTokenData RefreshTokenData { get; init; } = null!;
    }
}
