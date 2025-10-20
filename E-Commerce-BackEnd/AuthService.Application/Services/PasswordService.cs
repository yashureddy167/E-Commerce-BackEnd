using AuthService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services
{
    public class PasswordService: IPasswordService
    {
        private readonly PasswordHasher<object> _passwordHasher = new();

        public async Task<string> HashPasswordAsync(string password)
        {
            return _passwordHasher.HashPassword(new object(), password);
        }

        public async Task<bool> VerfiyPasswordAsync(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(new object(), hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
