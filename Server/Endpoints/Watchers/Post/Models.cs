using FluentValidation;

namespace Watchers.Post;

sealed class PostWatcherRequest
{
    public string Title { get; set; }
    public IEnumerable<string> Links { get; set; }
}

sealed class Validator : Validator<PostWatcherRequest>
{
    public Validator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(255);

        RuleForEach(x => x.Links)
            .NotEmpty()
            .Must(IsValidHost)
            .WithMessage("One or more links cannot be watched, due to not supported domain");
    }

    private static bool IsValidHost(string link)
    {
        var uri = new Uri(link);
        return Data.AvailableHosts.Contains(uri.Host);
    }
}