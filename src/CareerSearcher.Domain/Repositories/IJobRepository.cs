using CareerSearcher.Domain.Entities;

namespace CareerSearcher.Domain.Repositories;

public interface IJobRepository : IBaseRepository<Job>
{
    Task<ICollection<Job>> GetAllFromCompany(string company);
    Task SaveNewJobs(IEnumerable<Job> jobs);
}
