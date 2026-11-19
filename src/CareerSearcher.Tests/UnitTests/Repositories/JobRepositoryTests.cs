using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Repositories;
using CareerSearcher.Infrastructure.Persistence.Contexts;
using CareerSearcher.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CareerSearcher.Tests.UnitTests.Repositories;

[TestClass]
public class JobRepositoryTests
{
    private readonly AppDbContext       _dbContext;
    private readonly IJobRepository     _jobRepository;
    private readonly ITestOutputHelper  _outputHelper;
    public JobRepositoryTests(ITestOutputHelper outputHelper)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext      = new AppDbContext(options);
        _jobRepository  = new JobRepository(_dbContext);
        _outputHelper   = outputHelper;
    }

    [Fact]
    public async Task Should_Add_A_New_Job()
    {
        var job = new Job("Job Test", "Job Company", "Job Description", "https://www.job.com/");

        await _jobRepository.Add(job);

        bool saveResult = (await _dbContext.SaveChangesAsync(TestContext.Current.CancellationToken)) > 0;

        Assert.True(saveResult);
        Assert.True(await _dbContext.Jobs.AnyAsync(TestContext.Current.CancellationToken));

        _outputHelper.WriteLine($"""
                {job.Id} 
                {job.Name} 
                {job.Company} 
                {job.Description} 
                {job.Url} 
                {job.UniqueId} 
                {job.CreatedAt}
           """);
    }

    [Fact]
    public async Task Should_Search_Jobs_Across_Title_Company_And_Description()
    {
        await _jobRepository.Add(new Job("Backend Developer", "Northwind", "Build APIs", "https://example.com/1"));
        await _jobRepository.Add(new Job("Product Designer", "Backend Studio", "Design experiences", "https://example.com/2"));
        await _jobRepository.Add(new Job("QA Engineer", "Contoso", "Backend quality", "https://example.com/3"));
        await _dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var (jobs, amount) = await _jobRepository.GetPage("Backend", 1, 2);

        Assert.Equal(3, amount);
        Assert.Equal(2, jobs.Count);

        var (remainingJobs, remainingAmount) = await _jobRepository.GetPage("Backend", 2, 2);

        Assert.Equal(amount, remainingAmount);
        Assert.Single(remainingJobs);
    }
}
