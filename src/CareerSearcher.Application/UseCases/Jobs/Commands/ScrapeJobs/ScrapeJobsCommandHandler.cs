using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Interfaces;
using CareerSearcher.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CareerSearcher.Application.UseCases.Jobs.Commands.ScrapeJobs;

internal sealed class ScrapeJobsCommandHandler : IRequestHandler<ScrapeJobsCommand, IEnumerable<Job>>
{
    private readonly ILogger<ScrapeJobsCommandHandler>  _logger;
    private readonly IEnumerable<IJobScrapper>          _scrappers;
    private readonly IJobRepository                     _jobRepository;
    private readonly IUnitOfWork                        _unitOfWork;

    public ScrapeJobsCommandHandler(ILogger<ScrapeJobsCommandHandler> logger, 
        IEnumerable<IJobScrapper> scrappers, IJobRepository jobRepository, IUnitOfWork unitOfWork)
    {
        _logger                 = logger;
        _scrappers              = scrappers;
        _jobRepository          = jobRepository;
        _unitOfWork             = unitOfWork;
    }

    public async Task<IEnumerable<Job>> Handle(ScrapeJobsCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return default;

        _logger.LogInformation("Starting to scrape new jobs");

        List<Job> jobs = [];
        foreach(var scrapper in _scrappers)
        {
            var res = await scrapper.SearchAsync(request.Query);

            jobs.AddRange(res);
        }

        _logger.LogInformation("Done scrapping new jobs!");

        var newJobs = await _jobRepository.SaveNewJobs(jobs);
        
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        _logger.LogInformation("Saved all new scrapped jobs!");

        return newJobs;
    }
}
