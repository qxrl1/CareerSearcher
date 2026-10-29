using CareerSearcher.Application.Common.Behaviors;
using CareerSearcher.Application.Interfaces;
using CareerSearcher.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;

namespace CareerSearcher.Application.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        string discordWebhookUrl = configuration["WebhookUrl"]
            ?? throw new ArgumentNullException("WebhookUrl was not found");

        services.AddHttpClient<IDiscordWebhookService, DiscordWebhookService>(cfg =>
        {
            cfg.BaseAddress = new Uri(discordWebhookUrl);
            cfg.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }
}
