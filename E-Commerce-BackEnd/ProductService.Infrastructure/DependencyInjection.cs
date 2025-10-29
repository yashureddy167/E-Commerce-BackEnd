using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Infrastructure.Database;

namespace ProductService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Infrastructure service registrations go here
            services.AddDbContext<ProductDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("ProductDatabase");
                options.UseSqlServer(connectionString);
            });
            return services;
        }
    }
}
