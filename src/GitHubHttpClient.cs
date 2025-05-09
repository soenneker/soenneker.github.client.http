using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.GitHub.Client.Http.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.GitHub.Client.Http;

/// <inheritdoc cref="IGitHubHttpClient"/>
public sealed class GitHubHttpClient : IGitHubHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _config;

    private const string _clientId = nameof(GitHubHttpClient);

    public GitHubHttpClient(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;
        _config = config;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(_clientId, () =>
        {
            var token = _config.GetValueStrict<string>("GitHub:Token");

            return new HttpClientOptions
            {
                BaseAddress = "https://api.github.com/",
                DefaultRequestHeaders = new Dictionary<string, string>
                {
                    { "Accept", "application/vnd.github+json" },
                    { "Authorization", $"Bearer {token}" },
                    { "X-GitHub-Api-Version", "2022-11-28" },
                    { "User-Agent", Guid.NewGuid().ToString() }
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClientCache.RemoveSync(_clientId);
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _httpClientCache.Remove(_clientId);
    }
}