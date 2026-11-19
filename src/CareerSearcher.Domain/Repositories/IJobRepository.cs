using CareerSearcher.Domain.Entities;

namespace CareerSearcher.Domain.Repositories;

public interface IJobRepository : IBaseRepository<Job>
{
    Task<ICollection<Job>> GetAllFromCompany(string company);
    Task<(IReadOnlyList<Job> Jobs, int Amount)> GetPage(string? search, int page, int pageSize);
    Task<IReadOnlyList<Job>> SaveNewJobs(IEnumerable<Job> jobs);
}
