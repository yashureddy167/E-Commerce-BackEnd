using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Application.Interfaces.Services;

namespace AuthService.Application.Services
{
    public class TokenGenerationService: ITokenGenerationService
    {
        private readonly IConfiguration _configuration;

        public TokenGenerationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokensAsync(long userId, string email, string role)
        {
            var accessToken = await GenerateAccessTokenAsync(userId, email, role);
            var refreshToken = GenerateRefreshToken();
            return (accessToken, refreshToken);
        }

        private Task<string> GenerateAccessTokenAsync(long userId, string email, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var keyString = _configuration["JwtSettings:Key"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expirationMinutes = double.TryParse(_configuration["JwtSettings:AccessTokenExpirationMinutes"], out var minutes) ? minutes : 15;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        private string GenerateRefreshToken(int size = 64)
        {
            var randomNumber = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
