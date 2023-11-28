using System.Text.RegularExpressions;
using PriceWatcher.Implementations.Contracts.Scrapers;
using PriceWatcher.Implementations.Models;
using PuppeteerSharp;

namespace PriceWatcher.Implementations.Scrapers;

public partial class AmazonScraper : IScraper
{
    public Task GetPriceForItemsAsync(IBrowser browser, IEnumerable<ItemWatcher> watchers)
    {
        var workers = watchers.Select(watcher => Task.Run(() => GetPriceForSingleItemAsync(browser, watcher)));
        return Task.WhenAll(workers);
    }

    private static async Task GetPriceForSingleItemAsync(IBrowser browser, ItemWatcher watcher)
    {
        using var page = await browser.NewPageAsync();

        var supportedWatcherLinks = watcher.Links.Where(x => AmazonDomainPattern().IsMatch(x.Link));

        foreach (var watcherLink in supportedWatcherLinks)
        {
            await page.GoToAsync(watcherLink.Link);
        }
    }

    [GeneratedRegex(@"https?:\/\/(www.)?amazon.*")]
    private static partial Regex AmazonDomainPattern();
}
