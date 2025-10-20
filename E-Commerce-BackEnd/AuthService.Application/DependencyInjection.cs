using AuthService.Application.Interfaces.Services;
using AuthService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AuthService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<ITokenGenerationService, TokenGenerationService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
