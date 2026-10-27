using Microsoft.Playwright;

namespace CareerSearcher.Infrastructure.Browser;

internal sealed class BrowserContext
{
    private IPlaywright?    _playwright;
    private IBrowser?       _browser;

    public async Task<IBrowser> GetBrowser()
    {
        if (_browser is null)
        {
            _playwright = await Playwright.CreateAsync();
            _browser    = await _playwright.Chromium.LaunchAsync();
        }

        return _browser;
    }
}
