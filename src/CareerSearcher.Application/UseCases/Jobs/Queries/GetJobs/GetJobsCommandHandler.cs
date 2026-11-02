using CareerSearcher.Domain.Common;
using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Repositories;
using MediatR;

namespace CareerSearcher.Application.UseCases.Jobs.Queries.GetJobs;

internal sealed class GetJobsCommandHandler(IJobRepository _jobRepository) 
    : IRequestHandler<GetJobsCommand, Pagination<Job>>
{
    public async Task<Pagination<Job>> Handle(GetJobsCommand request, CancellationToken cancellationToken)
    {
        int page    = request.Page <= 0 ? 1 : request.Page;
        int amount  = await _jobRepository.Count();

        var jobs    = await _jobRepository.GetAll(page, request.PageSize);

        return new Pagination<Job>(page, request.PageSize, amount, [.. jobs]);
    }
}
