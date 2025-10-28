using AuthService.Application.DTO_s;
using MediatR;

namespace AuthService.Application.Features.Commands
{
    public record LoginCommand(LoginDTO loginDTO) : IRequest<(string accessToken, string refreshToken)>;
}
