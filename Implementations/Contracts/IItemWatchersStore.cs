namespace PriceWatcher.Implementations.Contracts;

public interface IItemWatcherStore<T> where T : class
{
    Task AddAsync(T item, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetBatchAsync(CancellationToken cancellationToken = default);
}
