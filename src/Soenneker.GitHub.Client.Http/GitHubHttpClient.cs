using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.GitHub.Client.Http.Abstract;
using Soenneker.HttpClients.LoggingHandler;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.GitHub.Client.Http;

public sealed class GitHubHttpClient : IGitHubHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _config;
    private readonly ILogger<GitHubHttpClient> _logger;

    private readonly string _clientId = $"{nameof(GitHubHttpClient)}:{Guid.NewGuid():N}";
    private readonly string _uploadClientId = $"{nameof(GitHubHttpClient)}:Upload:{Guid.NewGuid():N}";

    public GitHubHttpClient(IHttpClientCache httpClientCache, IConfiguration config, ILogger<GitHubHttpClient> logger)
    {
        _httpClientCache = httpClientCache;
        _config = config;
        _logger = logger;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return GetClient(_clientId, null, cancellationToken);
    }

    public ValueTask<HttpClient> GetForUpload(CancellationToken cancellationToken = default)
    {
        return GetClient(_uploadClientId, TimeSpan.FromMinutes(10), cancellationToken);
    }

    private ValueTask<HttpClient> GetClient(string clientId, TimeSpan? timeout, CancellationToken cancellationToken)
    {
        return _httpClientCache.Get(clientId, (config: _config, logger: _logger, timeout), static state =>
        {
            var token = state.config.GetValueStrict<string>("GH:Token");
            bool logging = state.config.GetValue<bool>("GH:RequestResponseLogging");

            return new HttpClientOptions
            {
                BaseAddress = new Uri("https://api.github.com/"),
                Timeout = state.timeout,
                DefaultRequestHeaders = new Dictionary<string, string>(4)
                {
                    { "Accept", "application/vnd.github+json" },
                    { "Authorization", $"Bearer {token}" },
                    { "X-GitHub-Api-Version", "2022-11-28" },
                    { "User-Agent", Guid.NewGuid().ToString() }
                },
                DelegatingHandlerFactories = logging
                    ?
                    [
                        () => new HttpClientLoggingHandler(state.logger, new HttpClientLoggingOptions
                        {
                            LogLevel = LogLevel.Debug,
                            RedactedHeaders = ["Authorization"]
                        })
                    ]
                    : null
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        try
        {
            _httpClientCache.RemoveSync(_clientId);
        }
        finally
        {
            _httpClientCache.RemoveSync(_uploadClientId);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await _httpClientCache.Remove(_clientId).ConfigureAwait(false);
        }
        finally
        {
            await _httpClientCache.Remove(_uploadClientId).ConfigureAwait(false);
        }
    }
}
