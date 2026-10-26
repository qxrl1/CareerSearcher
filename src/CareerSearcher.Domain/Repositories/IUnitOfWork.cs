namespace CareerSearcher.Domain.Repositories;

public interface IUnitOfWork
{
    Task<bool> CommitChangesAsync(CancellationToken cancellationToken = default);
}
