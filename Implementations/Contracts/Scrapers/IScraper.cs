using PriceWatcher.Implementations.Models;
using PuppeteerSharp;

namespace PriceWatcher.Implementations.Contracts.Scrapers;

public interface IScraper
{
    Task GetPriceForItemsAsync(IBrowser browser, IEnumerable<ItemWatcher> watchers);
}
