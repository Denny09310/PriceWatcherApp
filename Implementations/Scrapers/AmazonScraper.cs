using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using PriceWatcher.Implementations.Contracts.Scrapers;
using PriceWatcher.Implementations.Models;
using PuppeteerSharp;

namespace PriceWatcher.Implementations.Scrapers;

public partial class AmazonScraper(ILogger<AmazonScraper> logger) : IScraper
{
    public Task GetPriceForItemsAsync(IBrowser browser, IEnumerable<ItemWatcher> watchers)
    {
        var workers = watchers.Select(watcher => Task.Run(() => GetPriceForSingleItemAsync(browser, watcher)));
        return Task.WhenAll(workers);
    }

    private async Task GetPriceForSingleItemAsync(IBrowser browser, ItemWatcher watcher)
    {
        using var page = await browser.NewPageAsync();

        var supportedWatcherLinks = watcher.Links.Where(x => AmazonDomainPattern().IsMatch(x.Link));

        foreach (var watcherLink in supportedWatcherLinks)
        {
            await page.GoToAsync(watcherLink.Link);

            var priceElementHandle = await page.WaitForSelectorAsync("span.a-price > span.a-offscreen");
            var priceInnerText = await priceElementHandle.GetPropertyAsync("innerText");
            var priceValue = await priceInnerText.JsonValueAsync<string>();

            var culture = await page.EvaluateFunctionAsync<string>("() => document.documentElement.lang");
            var currentCulture = CultureInfo.GetCultureInfo(culture) ?? CultureInfo.CurrentCulture;

            if (!decimal.TryParse(priceValue, NumberStyles.Currency, currentCulture, out var price))
            {
                return;
            }

            logger.LogInformation("Price for item {Title} found: {Price}{CurrencySymbol}", watcher.Title, price, currentCulture.NumberFormat.CurrencySymbol);
        }
    }

    [GeneratedRegex(@"https?:\/\/(www.)?amazon.\w+/")]
    private static partial Regex AmazonDomainPattern();
}
