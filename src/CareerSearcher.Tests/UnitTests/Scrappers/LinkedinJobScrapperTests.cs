using CareerSearcher.Infrastructure.Scrappers;
using Xunit;

namespace CareerSearcher.Tests.UnitTests.Scrappers;

[TestClass]
public class LinkedinJobScrapperTests
{
    private readonly LinkedinJobScrapper _scrapper;
    private readonly ITestOutputHelper   _outputHelper;

    public LinkedinJobScrapperTests(ITestOutputHelper outputHelper)
    {
        _scrapper     = new LinkedinJobScrapper(new Infrastructure.Browser.BrowserContext());
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Should_Return_A_List_Of_Jobs()
    {
        string query = "Desenvolvedor .NET";

        var result = await _scrapper.SearchAsync(query);

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        foreach (var job in result)
            _outputHelper.WriteLine($"{job.Name} | {job.Company} | {job.Url} | {job.UniqueId}");
    }
}
