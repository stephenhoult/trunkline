using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Trunkline.Api.Tests;

public class LinkApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    // Redirects off, so the test sees the 302 itself rather than following it to the target.
    private readonly HttpClient _client = factory.CreateClient(new WebApplicationFactoryClientOptions
    {
        AllowAutoRedirect = false,
    });

    [Fact]
    public async Task Health_returns_200()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Created_link_redirects_to_its_url()
    {
        const string url = "https://example.com/some/page";

        var created = await _client.PostAsJsonAsync("/links", new { url });
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        var link = await created.Content.ReadFromJsonAsync<LinkResponse>();
        Assert.NotNull(link);

        var redirect = await _client.GetAsync($"/{link.Slug}");

        Assert.Equal(HttpStatusCode.Redirect, redirect.StatusCode);
        Assert.Equal(url, redirect.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task Unknown_slug_returns_404()
    {
        var response = await _client.GetAsync("/no-such-slug");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Invalid_url_returns_400_with_problem_details()
    {
        var response = await _client.PostAsJsonAsync("/links", new { url = "not a url" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }
}
