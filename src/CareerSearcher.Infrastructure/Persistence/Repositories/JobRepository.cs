using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Repositories;
using CareerSearcher.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CareerSearcher.Infrastructure.Persistence.Repositories;

internal sealed class JobRepository(AppDbContext _dbContext)
    : BaseRepository<Job>(_dbContext), IJobRepository
{
    public async Task<ICollection<Job>> GetAllFromCompany(string company)
    {
        return await _dbContext.Jobs
            .AsNoTracking()
            .Where(x => x.Company == company)
            .ToListAsync();
    }

    public async Task SaveNewJobs(IEnumerable<Job> jobs)
    {
        var deduplicatedJobs = jobs.DistinctBy(x => x.UniqueId);
    
        foreach (var job in deduplicatedJobs)
        {
            if (await _dbContext.Jobs.AnyAsync(x => x.UniqueId == job.UniqueId))
                continue;

            await _dbContext.Jobs.AddAsync(job);
        }
    }
}
