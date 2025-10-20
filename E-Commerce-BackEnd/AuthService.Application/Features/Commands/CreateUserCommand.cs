using AuthService.Application.DTO_s;
using MediatR;

namespace AuthService.Application.Features.Commands
{
    public record CreateUserCommand(UserDTO User): IRequest<bool>;
}
