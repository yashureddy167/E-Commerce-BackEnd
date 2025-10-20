namespace AuthService.Application.DTO_s
{
    public record UserDTO
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public DateTime DateOfBirth { get; init; }
        public bool IsTwoFactorAuthEnabled { get; init; } = false;
    }
}
