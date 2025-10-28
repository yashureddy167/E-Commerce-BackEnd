using AuthService.Application.Features.Commands;
using AuthService.Application.Interfaces.Repositories;
using AuthService.Application.Interfaces.Services;
using AuthService.Domain.Entities;
using MediatR;

namespace AuthService.Application.Features.CommandHandlers
{
    public class LoginCommandHandler(IPasswordService passwordService,
        ITokenGenerationService tokenGenerationService,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository) : IRequestHandler<LoginCommand, (string accessToken, string refreshToken)>
    {

        public async Task<(string accessToken, string refreshToken)> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await userRepository.GetUserByEmailAsync(request.loginDTO.Email, cancellationToken);
                if (user == null)
                {
                    throw new UnauthorizedAccessException("user does not exist");
                }
                var isPasswordValid = await passwordService.VerfiyPasswordAsync(user.PasswordHash, request.loginDTO.Password);
                if (!isPasswordValid)
                {
                    throw new UnauthorizedAccessException("invalid credentials");
                }
                var tokens = await tokenGenerationService.GenerateAccessAndRefreshTokensAsync(user.UserId, user.Email, user.Role);
                var newRefreshToken = new RefreshToken()
                {
                    Token = tokens.RefreshTokenData.Token,
                    ExpiresAt = tokens.RefreshTokenData.ExpiresAt,
                    CreatedAt = tokens.RefreshTokenData.CreatedAt,
                    UserId = tokens.RefreshTokenData.UserId
                };
                await refreshTokenRepository.AddTokenAsync(newRefreshToken);
                return (tokens.AccessToken, tokens.RefreshTokenData.Token);
            }
            catch
            {
                throw;
            }
        }
    }
}
