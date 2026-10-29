using CareerSearcher.Domain.Entities;
using CareerSearcher.Domain.Interfaces;
using CareerSearcher.Infrastructure.Browser;
using Microsoft.Playwright;
using System.Web;

namespace CareerSearcher.Infrastructure.Scrappers;

internal sealed class LinkedinJobScrapper(BrowserContext _browserContext) : IJobScrapper
{
    private readonly string URL = "https://br.linkedin.com/jobs/search?keywords={Query}&location=Brasil&geoId=106057199&position=1&pageNum=0";

    public async Task<IEnumerable<Job>> SearchAsync(string query)
    {
        var browser = await _browserContext.GetBrowser();
        var context = await browser.NewContextAsync();
        var page    = await context.NewPageAsync();

        string formattedURL = URL.Replace("{Query}", HttpUtility.UrlEncode(query));

        await page.GotoAsync(formattedURL, new() { WaitUntil = WaitUntilState.DOMContentLoaded });

        var items = await page.Locator(".jobs-search__results-list > li").AllAsync();

        List<Job> jobs = [];
        foreach (var item in items)
        {
            var title   = await item.Locator("h3").InnerTextAsync();
            var company = await item.Locator("h4").InnerTextAsync();
            var url     = await item.Locator("a").Nth(0).GetAttributeAsync("href");

            if (title is null || company is null || url is null)
                continue;

            jobs.Add(new Job(title, company, "", url));
        }

        await page.CloseAsync();
        await context.CloseAsync();

        return jobs;
    }
}
