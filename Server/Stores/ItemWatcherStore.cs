using Microsoft.EntityFrameworkCore;
using PriceWatcher.Implementations.Contracts;
using PriceWatcher.Implementations.Models;
using PriceWatcher.Server.Data;

namespace PriceWatcher.Server.Stores;

public class ItemWatcherStore(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IItemWatcherStore<ItemWatcher>
{
    private static readonly TimeSpan CheckOffset = TimeSpan.FromMinutes(1);

    public async Task AddAsync(ItemWatcher item, CancellationToken cancellationToken = default)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.ItemWatchers.AddAsync(item, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ItemWatcher>> GetBatchAsync(CancellationToken cancellationToken = default)
    {
        using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return dbContext.ItemWatchers
            .Include(x => x.Links)
            .AsEnumerable()
            .Where(x => x.LastCheckAt is null || (DateTime.Now - x.LastCheckAt) > CheckOffset)
            .ToArray();
    }
}
