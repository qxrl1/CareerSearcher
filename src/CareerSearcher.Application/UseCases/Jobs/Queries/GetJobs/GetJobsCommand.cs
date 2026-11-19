using CareerSearcher.Domain.Common;
using CareerSearcher.Domain.Entities;
using MediatR;

namespace CareerSearcher.Application.UseCases.Jobs.Queries.GetJobs;

public sealed record GetJobsCommand(
    int Page = 1,
    int PageSize = 10,
    string? Search = null
) : IRequest<Result<Pagination<Job>>>;
