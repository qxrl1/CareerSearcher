using CareerSearcher.Application.Interfaces;
using CareerSearcher.Application.UseCases.Jobs.Commands.ScrapeJobs;
using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Interfaces;
using CareerSearcher.Domain.Repositories;
using CareerSearcher.Infrastructure.Persistence.Contexts;
using CareerSearcher.Infrastructure.Persistence.Repositories;
using CareerSearcher.Infrastructure.Scrappers;
using CareerSearcher.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CareerSearcher.Tests.UnitTests.Handlers;

[TestClass]
public class ScrapeJobsCommandHandlerTests
{
    private readonly ScrapeJobsCommandHandler   _handler;
    private readonly AppDbContext               _dbContext;
    private readonly IJobRepository             _jobRepository;
    private readonly IUnitOfWork                _unitOfWork;
    private readonly Mock<IJobNotificationQueue> _jobNotificationQueueMock = new Mock<IJobNotificationQueue>();

    private readonly List<Job> _mockData = [new("dsdas", "dsadsa", "dsadas", "dsadas")];

    private readonly IEnumerable<IJobScrapper>  _scrappers = new List<IJobScrapper>()
    {
        new LinkedinJobScrapper(new Infrastructure.Browser.BrowserContext())
    };

    public ScrapeJobsCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext      = new AppDbContext(options);
        _jobRepository  = new JobRepository(_dbContext);
        _unitOfWork     = new UnitOfWork(_dbContext);

        _jobNotificationQueueMock.Setup(x => x.EnqueueNotification(It.IsAny<Job>()));
        _jobNotificationQueueMock.Setup(x => x.Dequeue(out It.Ref<Job>.IsAny)).Returns(true);

        _handler = new ScrapeJobsCommandHandler(new Logger<ScrapeJobsCommandHandler>(), _scrappers, _jobRepository, _unitOfWork);
    }

    [Fact]
    public async Task Should_Scrape_New_Jobs_And_Save_To_The_Database()
    {
        string query = "Desenvolvedor .NET";
        var command = new ScrapeJobsCommand(query);

        await _handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.True(await _dbContext.Jobs.AnyAsync(TestContext.Current.CancellationToken));
    }
}
