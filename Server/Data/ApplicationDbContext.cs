using Microsoft.EntityFrameworkCore;
using PriceWatcher.Implementations.Models;

namespace PriceWatcher.Server.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ItemWatcher> ItemWatchers => Set<ItemWatcher>();
    public DbSet<ItemWatcherLink> ItemLinks => Set<ItemWatcherLink>();
}
