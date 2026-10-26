using CareerSearcher.Domain.Entities;

namespace CareerSearcher.Domain.Interfaces;

internal interface IJobScrapper
{
    Task<IEnumerable<Job>> SearchAsync(string query);
}
