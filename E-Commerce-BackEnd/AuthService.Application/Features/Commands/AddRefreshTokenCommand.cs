using MediatR;

namespace AuthService.Application.Features.Commands
{
    public record AddRefreshTokenCommand(string refreshToken): IRequest;
}
