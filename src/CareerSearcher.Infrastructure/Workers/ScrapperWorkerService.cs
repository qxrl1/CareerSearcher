using CareerSearcher.Application.UseCases.Jobs.Commands.ScrapeJobs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CareerSearcher.Infrastructure.Workers;

internal sealed class ScrapperWorkerService : BackgroundService
{
    private readonly ILogger<ScrapperWorkerService> _logger;
    private readonly IServiceScopeFactory           _serviceScopeFactory;

    private readonly TimeSpan DELAY_IN_MINUTES = TimeSpan.FromMinutes(5);

    private readonly string[] DEFAULT_QUERIES = [
        "Desenvolvedor",
        "Desenvolvedor .NET",
        "Desenvolvedor Backend"
    ];

    public ScrapperWorkerService(ILogger<ScrapperWorkerService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger              = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.Register(() => _logger.LogInformation("Stopping Scrapper Worker..."));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await StartScrapper(stoppingToken);
            } catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            } catch (Exception ex)
            {
                _logger.LogError("Something Bad Happened: {Error}", ex.Message);
            }

            await Task.Delay(DELAY_IN_MINUTES, stoppingToken);
        }
    }

    private async Task StartScrapper(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        foreach (var query in DEFAULT_QUERIES)
            await mediator.Send(new ScrapeJobsCommand(query), cancellationToken);
    }
}
