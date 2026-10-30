using CareerSearcher.Application.Interfaces;
using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CareerSearcher.Application.EventHandlers;

internal sealed class ScrapingFinishedEventHandler : INotificationHandler<ScrapingFinishedEvent>
{
    private readonly ILogger<ScrapingFinishedEventHandler>  _logger;
    private readonly IDiscordNotificationService            _discordNotificationService;

    public ScrapingFinishedEventHandler(ILogger<ScrapingFinishedEventHandler> logger,
        IDiscordNotificationService discordNotificationService)
    {
        _logger                     = logger;
        _discordNotificationService = discordNotificationService;
    }

    public async Task Handle(ScrapingFinishedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling ScrapingFinishedEvent");

        _ = Task.Factory.StartNew(async () =>
        {
            foreach (var job in notification.Jobs)
            {
                await _discordNotificationService.SendAsync(job, cancellationToken);
                await Task.Delay(5000);
            }

            _logger.LogInformation("Done sending all notifications");
        }).Unwrap();

        _logger.LogInformation("Handled ScrapingFinishedEvent successfully!");
    }
}
