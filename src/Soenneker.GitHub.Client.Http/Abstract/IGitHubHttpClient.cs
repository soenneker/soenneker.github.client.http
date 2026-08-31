using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.GitHub.Client.Http.Abstract;

/// <summary>
/// Provides the configured HTTP client used to call GitHub's REST API.
/// </summary>
public interface IGitHubHttpClient : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the GitHub HTTP client owned by this provider.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel retrieval.</param>
    /// <returns>The configured HTTP client.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
