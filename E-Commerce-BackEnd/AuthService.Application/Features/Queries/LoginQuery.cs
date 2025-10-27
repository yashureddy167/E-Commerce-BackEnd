using AuthService.Application.DTO_s;
using AuthService.Domain.Data;
using MediatR;

namespace AuthService.Application.Features.Queries
{
    public record LoginQuery(LoginDTO loginDTO): IRequest<(string accessToken, string refreshToken)>;
}
