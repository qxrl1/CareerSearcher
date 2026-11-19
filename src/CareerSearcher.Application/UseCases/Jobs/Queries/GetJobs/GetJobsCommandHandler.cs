using CareerSearcher.Domain.Common;
using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Repositories;
using MediatR;

namespace CareerSearcher.Application.UseCases.Jobs.Queries.GetJobs;

internal sealed class GetJobsCommandHandler(IJobRepository _jobRepository) 
    : IRequestHandler<GetJobsCommand, Result<Pagination<Job>>>
{
    public async Task<Result<Pagination<Job>>> Handle(GetJobsCommand request, CancellationToken cancellationToken)
    {
        int page    = request.Page <= 0 ? 1 : request.Page;
        int pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var (jobs, amount) = await _jobRepository.GetPage(request.Search, page, pageSize);
        
        var pagination = new Pagination<Job>(page, pageSize, amount, [.. jobs]);

        return pagination;
    }
}
