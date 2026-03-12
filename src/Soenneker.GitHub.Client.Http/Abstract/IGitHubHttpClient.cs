using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.GitHub.Client.Http.Abstract;

/// <summary>
/// A .NET thread-safe singleton HttpClient for GitHub
/// </summary>
public interface IGitHubHttpClient : IDisposable, IAsyncDisposable
{
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}