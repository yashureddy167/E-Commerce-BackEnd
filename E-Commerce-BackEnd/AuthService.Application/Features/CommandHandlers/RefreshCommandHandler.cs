using AuthService.Application.Features.Commands;
using AuthService.Application.Interfaces.Repositories;
using AuthService.Application.Interfaces.Services;
using MediatR;

namespace AuthService.Application.Features.CommandHandlers
{
    public class RefreshCommandHandler(IRefreshTokenRepository refreshTokenRepository,
        ITokenGenerationService tokenGenerationService,
        IUserRepository userRepository): IRequestHandler<RefreshCommand, (string accessToken, string refreshToken)>
    {

        public async Task<(string accessToken, string refreshToken)> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var existingRefreshToken = await refreshTokenRepository.GetRefreshTokenDetailsAsync(request.refreshToken);
            if (existingRefreshToken == null || existingRefreshToken.IsExpired || !existingRefreshToken.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }
            var existingUser = await userRepository.GetUserByUserIdAsync(existingRefreshToken.UserId, cancellationToken);
            if(existingUser == null) {
                throw new UnauthorizedAccessException("User not found.");
            }
            var tokens = await tokenGenerationService.GenerateAccessAndRefreshTokensAsync(existingUser.UserId, existingUser.Email, existingUser.Role);
            existingRefreshToken.Token = tokens.RefreshTokenData.Token;
            existingRefreshToken.CreatedAt = tokens.RefreshTokenData.CreatedAt;
            existingRefreshToken.ExpiresAt = tokens.RefreshTokenData.ExpiresAt;
            await refreshTokenRepository.UpdateTokenDetailsAsync(existingRefreshToken);
            (string accessToken, string refreshToken) = (tokens.AccessToken, tokens.RefreshTokenData.Token);
            return (accessToken, refreshToken);
        }
    }
}
