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
}
