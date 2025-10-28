using MediatR;

namespace AuthService.Application.Features.Commands
{
    public record LogoutCommand(string RefreshToken) : IRequest;
}
