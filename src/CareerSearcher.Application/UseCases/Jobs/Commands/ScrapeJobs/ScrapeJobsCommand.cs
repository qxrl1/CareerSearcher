using CareerSearcher.Domain.Common;
using CareerSearcher.Domain.Entities;
using MediatR;

namespace CareerSearcher.Application.UseCases.Jobs.Commands.ScrapeJobs;

// Scrape new jobs and save it to the database
public sealed record ScrapeJobsCommand(string Query) : IRequest<Result<IReadOnlyList<Job>>>;
