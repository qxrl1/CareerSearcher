using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CareerSearcher.Infrastructure.Persistence.Repositories;

internal abstract class BaseRepository<T>(DbContext _dbContext)
    : IBaseRepository<T> where T : BaseEntity
{
    private DbSet<T> _dbSet => _dbContext.Set<T>();

    public async Task Add(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<T?> Get(Guid Id)
    {
        return await _dbSet.FindAsync(Id);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
