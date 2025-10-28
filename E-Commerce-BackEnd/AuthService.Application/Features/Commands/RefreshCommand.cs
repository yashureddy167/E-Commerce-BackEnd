using MediatR;

namespace AuthService.Application.Features.Commands
{
    public record RefreshCommand(string refreshToken) : IRequest<(string accessToken, string refreshToken)>;
}
