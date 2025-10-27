using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Application.Interfaces.Services;
using System.Numerics;
using AuthService.Domain.Data;

namespace AuthService.Application.Services
{
    public class TokenGenerationService: ITokenGenerationService
    {
        private readonly IConfiguration _configuration;

        public TokenGenerationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TokensData> GenerateAccessAndRefreshTokensAsync(BigInteger userId, string email, string role)
        {
            var accessToken = await GenerateAccessTokenAsync(userId, email, role);
            var refreshToken = GenerateRefreshTokenAsync(userId);
            var tokensData = new TokensData
            {
                AccessToken = accessToken,
                RefreshTokenData = refreshToken
            };
            return tokensData;
        }

        private Task<string> GenerateAccessTokenAsync(BigInteger userId, string email, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var keyString = _configuration["JwtSettings:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("JWT key is not configured.");

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

        private RefreshTokenData GenerateRefreshTokenAsync(BigInteger userId,int size = 64)
        {
            var randomNumber = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            string refreshToken =  Convert.ToBase64String(randomNumber);
            return new RefreshTokenData
            {
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = userId
            };
        }
    }
}
