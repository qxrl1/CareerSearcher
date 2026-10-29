using CareerSearcher.Application.Interfaces;
using CareerSearcher.Domain.Interfaces;
using CareerSearcher.Domain.Repositories;
using CareerSearcher.Infrastructure.Browser;
using CareerSearcher.Infrastructure.Notification;
using CareerSearcher.Infrastructure.Persistence.Contexts;
using CareerSearcher.Infrastructure.Persistence.Repositories;
using CareerSearcher.Infrastructure.Scrappers;
using CareerSearcher.Infrastructure.Workers;
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

        services.AddSingleton<BrowserContext>();

        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddTransient<IJobScrapper, LinkedinJobScrapper>();

        services.AddHostedService<ScrapperWorkerService>();

        return services;
    }
}
