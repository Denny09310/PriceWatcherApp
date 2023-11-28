using PriceWatcher.Implementations.Models;

namespace Watchers.Post;

sealed class Mapper : Mapper<PostWatcherRequest, EmptyResponse, ItemWatcher>
{
    public override ItemWatcher ToEntity(PostWatcherRequest req)
    {
        return new()
        {
            Title = req.Title,
            Links = ToEntity(req.Links)
        };
    }

    private static HashSet<ItemWatcherLink> ToEntity(IEnumerable<string> links) =>
        links.Select(link => new ItemWatcherLink { Link = link })
             .ToHashSet();
}