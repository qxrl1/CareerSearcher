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

    public async Task<(IReadOnlyList<Job> Jobs, int Amount)> GetPage(string? search, int page, int pageSize)
    {
        var jobs = _dbContext.Jobs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var keywords = search.Trim();
            jobs = jobs.Where(job => job.Name.Contains(keywords) || job.Company.Contains(keywords) || job.Description.Contains(keywords));
        }

        var amount = await jobs.CountAsync();
        var data = await jobs
            .OrderByDescending(job => job.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, amount);
    }

    public async Task<IReadOnlyList<Job>> SaveNewJobs(IEnumerable<Job> jobs)
    {
        var deduplicatedJobs = jobs.DistinctBy(x => x.UniqueId);
        List<Job> newJobs = [];

        foreach (var job in deduplicatedJobs)
        {
            if (await _dbContext.Jobs.AnyAsync(x => x.UniqueId == job.UniqueId))
                continue;

            await _dbContext.Jobs.AddAsync(job);

            newJobs.Add(job);
        }

        return newJobs;
    }
}
