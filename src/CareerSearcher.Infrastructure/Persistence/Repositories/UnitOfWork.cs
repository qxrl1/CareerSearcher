using CareerSearcher.Domain.Repositories;
using CareerSearcher.Infrastructure.Persistence.Contexts;

namespace CareerSearcher.Infrastructure.Persistence.Repositories;

internal sealed class UnitOfWork(AppDbContext _dbContext) : IUnitOfWork
{
    public async Task<bool> CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        return (await _dbContext.SaveChangesAsync(cancellationToken)) > 0;
    }
}
