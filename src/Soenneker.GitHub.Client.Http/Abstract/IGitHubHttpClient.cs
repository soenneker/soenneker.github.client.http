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

    /// <summary>
    /// Gets a separate cached GitHub HTTP client with a 10-minute overall request timeout for asset uploads.
    /// </summary>
    /// <remarks>
    /// Uses the same authentication and logging settings as the regular client. Callers must not dispose the returned client.
    /// Request cancellation tokens can cancel uploads before the timeout expires.
    /// </remarks>
    /// <param name="cancellationToken">Token used to cancel retrieval.</param>
    /// <returns>The upload HTTP client owned by this provider.</returns>
    ValueTask<HttpClient> GetForUpload(CancellationToken cancellationToken = default);
}
