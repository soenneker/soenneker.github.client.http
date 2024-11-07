using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.GitHub.Client.Http.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Utils.HttpClientCache.Dtos;

namespace Soenneker.GitHub.Client.Http;

///<inheritdoc cref="IGitHubHttpClient"/>
public class GitHubHttpClient : IGitHubHttpClient
{
    private readonly IHttpClientCache _httpClientCache;

    private readonly HttpClientOptions _options = new() { BaseAddress = "https://api.github.com/" };

    public GitHubHttpClient(IHttpClientCache httpClientCache)
    {
        _httpClientCache = httpClientCache;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(nameof(GitHubHttpClient), _options, cancellationToken: cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _httpClientCache.RemoveSync(nameof(GitHubHttpClient));
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        return _httpClientCache.Remove(nameof(GitHubHttpClient));
    }
}