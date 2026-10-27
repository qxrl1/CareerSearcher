using CareerSearcher.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CareerSearcher.Infrastructure.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connString = configuration.GetConnectionString("SqlConnection")
            ?? throw new ArgumentNullException("SqlConnection not found");

        services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(connString, x => x.EnableRetryOnFailure()));

        return services;
    }
}
