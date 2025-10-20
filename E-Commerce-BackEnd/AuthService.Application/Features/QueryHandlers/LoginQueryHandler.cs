using AuthService.Application.Features.Queries;
using AuthService.Application.Interfaces.Repositories;
using AuthService.Application.Interfaces.Services;
using MediatR;

namespace AuthService.Application.Features.QueryHandlers
{
    public class LoginQueryHandler(IPasswordService passwordService,
        ITokenGenerationService tokenGenerationService,
        IUserRepository userRepository): IRequestHandler<LoginQuery, (string accessToken, string refreshToken)>
    {

        public async Task<(string, string)> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await userRepository.GetUserByEmailAsync(request.loginDTO.Email, cancellationToken);
                if(user == null)
                {
                    throw new Exception("user does not exist");
                }
                var isPasswordValid = await passwordService.VerfiyPasswordAsync(user.PasswordHash, request.loginDTO.Password);
                if (!isPasswordValid) {
                    throw new Exception("invalid credentials");
                }
                var tokens = await tokenGenerationService.GenerateAccessAndRefreshTokensAsync(user.UserId, user.Email, user.Role);
                return tokens;
            }
            catch
            {
                throw;
            }
        }
    }
}
