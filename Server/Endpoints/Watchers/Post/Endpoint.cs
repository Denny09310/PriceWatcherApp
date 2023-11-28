using PriceWatcher.Implementations.Contracts;
using PriceWatcher.Implementations.Models;

namespace Watchers.Post;

sealed class PostWatcherEndpoint(IItemWatcherStore<ItemWatcher<string>> store) : Endpoint<PostWatcherRequest, EmptyResponse, Mapper>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/watchers");
    }

    public override async Task HandleAsync(PostWatcherRequest req, CancellationToken ct)
    {
        var watcher = Map.ToEntity(req);
        await store.AddAsync(watcher, ct);

        await SendNoContentAsync(ct);
    }
}