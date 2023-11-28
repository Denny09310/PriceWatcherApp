using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PriceWatcher.Implementations.Contracts.Scrapers;
using PriceWatcher.Implementations.Models;
using PriceWatcher.Server.Data;
using PuppeteerSharp;

namespace PriceWatcher.Server.Scrapers;

public partial class AmazonScraper(ILogger<AmazonScraper> logger, IDbContextFactory<ApplicationDbContext> dbContextFactory) : IScraper
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

        using var dbContext = await dbContextFactory.CreateDbContextAsync();

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
                logger.LogWarning("Price for item '{Title}' not found", watcher.Title);
                continue;
            }

            logger.LogInformation("Price for item '{Title}' found: {Price}{CurrencySymbol}", watcher.Title, price, currentCulture.NumberFormat.CurrencySymbol);

            watcherLink.Price = price;
            dbContext.Entry(watcherLink).State = EntityState.Modified;
        }

        watcher.LastCheckAt = DateTime.Now;
        dbContext.Entry(watcher).State = EntityState.Modified;

        await dbContext.SaveChangesAsync();
    }

    [GeneratedRegex(@"https?:\/\/(www.)?amazon.\w+/")]
    private static partial Regex AmazonDomainPattern();
}
