using CareerSearcher.Domain.Entities;

namespace CareerSearcher.Domain.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task Add(T entity);
    Task<T?> Get(Guid Id);
    Task<IEnumerable<T>> GetAll(int offset, int count);
    Task<int> Count();
    void Delete(T entity);
    void Update(T entity);
}
