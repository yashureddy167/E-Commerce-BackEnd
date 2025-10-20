namespace AuthService.Application.DTO_s
{
    public record LoginDTO
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
