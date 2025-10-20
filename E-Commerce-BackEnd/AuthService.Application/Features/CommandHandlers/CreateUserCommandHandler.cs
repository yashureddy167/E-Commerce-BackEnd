using AuthService.Application.Features.Commands;
using AuthService.Application.Interfaces.Repositories;
using AuthService.Application.Interfaces.Services;
using AuthService.Domain.Entities;
using MediatR;

namespace AuthService.Application.Features.CommandHandlers
{
    public class CreateUserCommandHandler(IUserRepository userRepository,
        IPasswordService passwordService) : IRequestHandler<CreateUserCommand, bool>
    {
        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newUser = new User()
                {
                    CreatedAt = DateTime.UtcNow,
                    Email = request.User.Email,
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    DateOfBirth = request.User.DateOfBirth,
                    LastUpdatedAt = null,
                    IsTwoFactorAuthEnabled = request.User.IsTwoFactorAuthEnabled,
                    PasswordHash = await passwordService.HashPasswordAsync(request.User.Password)
                };

                var isUserAdded = await userRepository.AddUserAsync(newUser, cancellationToken);
                return isUserAdded;
            }
            catch
            {
                throw;
            }
        }
    }
}
