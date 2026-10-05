using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace Trunkline.Api;

public sealed record Link(string Slug, string Url, DateTimeOffset CreatedAtUtc);

public sealed record CreateLinkRequest(string? Url);

public sealed record LinkResponse(string Slug, string Url, string ShortUrl);

// Registered as a singleton, so one instance serves every request on every thread
// for the life of the process. Replaced by a database in ticket 013.
public sealed class LinkStore
{
    private const string SlugAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    // Longer than any literal route ("version" is 7), so a generated slug can never be
    // shadowed by one.
    private const int SlugLength = 8;
    private const int MaxAttempts = 5;

    private readonly ConcurrentDictionary<string, Link> _links = new();

    public Link Add(string url)
    {
        for (var attempt = 0; attempt < MaxAttempts; attempt++)
        {
            var link = new Link(RandomNumberGenerator.GetString(SlugAlphabet, SlugLength), url, DateTimeOffset.UtcNow);

            // TryAdd never overwrites: on a collision it returns false and we try another slug.
            if (_links.TryAdd(link.Slug, link))
            {
                return link;
            }
        }

        throw new InvalidOperationException($"Couldn't generate a unique slug in {MaxAttempts} attempts.");
    }

    public Link? Find(string slug) => _links.TryGetValue(slug, out var link) ? link : null;
}
