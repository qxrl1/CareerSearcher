using CareerSearcher.Application.UseCases.Jobs.Commands.ScrapeJobs;
using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace CareerSearcher.Infrastructure.Workers;

internal sealed class ScrapperWorkerService : BackgroundService
{
    private readonly ILogger<ScrapperWorkerService> _logger;
    private readonly IServiceScopeFactory           _serviceScopeFactory;

    private readonly TimeSpan DELAY_IN_MINUTES = TimeSpan.FromMinutes(30);

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

        List<Job> jobs = [];
        foreach (var query in DEFAULT_QUERIES)
        {
            var res = await mediator.Send(new ScrapeJobsCommand(query), cancellationToken);

            if (res.IsSuccess)
                jobs.AddRange(res.Value!);
        }

        await mediator.Publish(new ScrapingFinishedEvent(jobs));
    }
}
