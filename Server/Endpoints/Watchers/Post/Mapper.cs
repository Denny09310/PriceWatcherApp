using PriceWatcher.Implementations.Models;

namespace Watchers.Post;

sealed class Mapper : Mapper<PostWatcherRequest, EmptyResponse, ItemWatcher<string>>
{
    public override ItemWatcher<string> ToEntity(PostWatcherRequest req)
    {
        return new()
        {
            Title = req.Title,
            Links = ToEntity(req.Links)
        };
    }

    private static HashSet<ItemWatcherLink<string>> ToEntity(IEnumerable<string> links) =>
        links.Select(link => new ItemWatcherLink<string> { Link = link })
             .ToHashSet();
}