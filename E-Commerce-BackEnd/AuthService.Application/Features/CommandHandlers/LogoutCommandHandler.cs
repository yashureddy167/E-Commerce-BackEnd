using AuthService.Application.Features.Commands;
using AuthService.Application.Interfaces.Repositories;
using MediatR;

namespace AuthService.Application.Features.CommandHandlers
{
    public class LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository): IRequestHandler<LogoutCommand>
    {
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var existingRefreshTokenDetails = await refreshTokenRepository.GetRefreshTokenDetailsAsync(request.RefreshToken);
            if (existingRefreshTokenDetails is not null)
            {
                existingRefreshTokenDetails.RevokedAt = DateTime.UtcNow;
                await refreshTokenRepository.UpdateTokenDetailsAsync(existingRefreshTokenDetails);
            }
        }
    }
}
