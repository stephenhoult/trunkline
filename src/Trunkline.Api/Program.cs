using System.Reflection;
using Trunkline.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<LinkStore>();

var app = builder.Build();

// Stamped into the assembly at build time by the SDK: "<version>+<git commit SHA>".
var buildVersion = typeof(Program).Assembly
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
    .InformationalVersion ?? "unknown";

app.MapHealthChecks("/health");

app.MapGet("/version", (IHostEnvironment environment) => new
{
    Version = buildVersion,
    Environment = environment.EnvironmentName,
});

app.MapPost("/links", (CreateLinkRequest request, LinkStore store, HttpRequest http) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var uri)
        || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["url"] = ["Must be an absolute http or https URL."],
        });
    }

    var link = store.Add(request.Url);
    return Results.Created($"/links/{link.Slug}", ToResponse(link, http));
});

app.MapGet("/links/{slug}", (string slug, LinkStore store, HttpRequest http) =>
    store.Find(slug) is { } link ? Results.Ok(ToResponse(link, http)) : Results.NotFound());

app.MapGet("/{slug}", (string slug, LinkStore store) =>
    store.Find(slug) is { } link ? Results.Redirect(link.Url) : Results.NotFound());

app.Run();

static LinkResponse ToResponse(Link link, HttpRequest http) =>
    new(link.Slug, link.Url, $"{http.Scheme}://{http.Host}/{link.Slug}");
