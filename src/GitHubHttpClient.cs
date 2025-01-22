using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Extensions.Configuration;
using Soenneker.GitHub.Client.Http.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Utils.HttpClientCache.Dtos;

namespace Soenneker.GitHub.Client.Http;

///<inheritdoc cref="IGitHubHttpClient"/>
public class GitHubHttpClient : IGitHubHttpClient
{
    private readonly IHttpClientCache _httpClientCache;

    private readonly HttpClientOptions _options;

    public GitHubHttpClient(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;

        var token = config.GetValueStrict<string>("GitHub:Token");

        _options = new HttpClientOptions
        {
            BaseAddress = "https://api.github.com/",
            DefaultRequestHeaders = new Dictionary<string, string>
            {
                {"Accept", "application/vnd.github+json"},
                {"Authorization", $"Bearer {token}"},
                {"X-GitHub-Api-Version", "2022-11-28"},
                {"User-Agent", Guid.NewGuid().ToString()}
            }
        };
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