using CareerSearcher.Domain.Entities;

namespace CareerSearcher.Domain.Interfaces;

public interface IJobScrapper
{
    Task<IEnumerable<Job>> SearchAsync(string query);
}
