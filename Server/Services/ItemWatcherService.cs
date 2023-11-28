
using System.Diagnostics;
using PriceWatcher.Implementations.Contracts;
using PriceWatcher.Implementations.Contracts.Scrapers;
using PriceWatcher.Implementations.Models;
using PuppeteerSharp;

namespace PriceWatcher.Server.Services;

public class ItemWatcherService(ILogger<ItemWatcherService> logger, IServiceScopeFactory factory) : BackgroundService
{
    private IBrowser _browser = default!;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await new BrowserFetcher().DownloadAsync();
        _browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopwatch = Stopwatch.StartNew();

        while (!stoppingToken.IsCancellationRequested)
        {
            stopwatch.Restart();

            using var scope = factory.CreateScope();

            var itemWatcherStore = scope.ServiceProvider.GetRequiredService<IItemWatcherStore<ItemWatcher<string>>>();

            logger.LogInformation("Getting next watcher batch");
            var watchers = await itemWatcherStore.GetBatchAsync(stoppingToken);

            if (!watchers.Any())
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                continue;
            }

            logger.LogInformation("Analyzing batch of {WatcherCount}", watchers.Count());

            var scrapers = scope.ServiceProvider.GetServices<IScraper>();

            foreach (var scraper in scrapers)
            {
                await scraper.GetPriceForItemsAsync(_browser, watchers);
            }

            stopwatch.Stop();
            logger.LogInformation("Analisys finished in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _browser.CloseAsync();
        await base.StopAsync(cancellationToken);
    }
}
