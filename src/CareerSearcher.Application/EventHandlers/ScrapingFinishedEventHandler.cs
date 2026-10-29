using CareerSearcher.Application.Interfaces;
using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CareerSearcher.Application.EventHandlers;

internal sealed class ScrapingFinishedEventHandler : INotificationHandler<ScrapingFinishedEvent>
{
    private readonly ILogger<ScrapingFinishedEventHandler>  _logger;
    private readonly IDiscordWebhookService                 _discordWebhookService;

    public ScrapingFinishedEventHandler(ILogger<ScrapingFinishedEventHandler> logger, 
        IDiscordWebhookService discordWebhookService)
    {
        _logger                 = logger;
        _discordWebhookService  = discordWebhookService;
    }

    public async Task Handle(ScrapingFinishedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling ScrapingFinishedEvent");

        _ = Task.Factory.StartNew(async () =>
        {
            foreach (var job in notification.Jobs)
            {
                await _discordWebhookService.Send(job, cancellationToken);
                await Task.Delay(5000);
            }

            _logger.LogInformation("Done sending all notifications");
        }).Unwrap();

        _logger.LogInformation("Handled ScrapingFinishedEvent successfully!");
    }
}
