using PriceWatcher.Implementations.Contracts;
using PriceWatcher.Implementations.Contracts.Scrapers;
using PriceWatcher.Implementations.Models;
using PriceWatcher.Server.Scrapers;
using PriceWatcher.Server.Services;
using PriceWatcher.Server.Stores;

namespace PriceWatcher.Server.Extensions;

static class ItemWatcherExtensions
{
    public static IServiceCollection RegisterItemWatcher(this IServiceCollection services)
    {
        services.AddScoped<IItemWatcherStore<ItemWatcher>, ItemWatcherStore>();
        services.AddScoped<IScraper, AmazonScraper>();
        services.AddHostedService<ItemWatcherService>();

        return services;
    }
}
