using CareerSearcher.Infrastructure.Browser;
using Microsoft.Playwright;
using Xunit;

namespace CareerSearcher.Tests.UnitTests.Browser;

[TestClass]
public class BrowserContextTests
{
    private readonly BrowserContext _browserContext;

    public BrowserContextTests()
    {
        _browserContext = new BrowserContext();
    }

    [Fact]
    public async Task Should_Create_A_Chromium_Browser()
    {
        var browser = await _browserContext.GetBrowser();

        Assert.NotNull(browser);
        Assert.Equal(BrowserType.Chromium, browser.BrowserType.Name);
    }
}
