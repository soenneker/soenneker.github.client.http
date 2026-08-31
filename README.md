[![](https://img.shields.io/nuget/v/soenneker.github.client.http.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.github.client.http/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.github.client.http/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.github.client.http/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.github.client.http.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.github.client.http/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.github.client.http/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.github.client.http/actions/workflows/codeql.yml)

# Soenneker.GitHub.Client.Http

Provides a cached `HttpClient` configured for GitHub's REST API, media type, API version, and bearer authentication.

## Installation

```bash
dotnet add package Soenneker.GitHub.Client.Http
```

## Configure and register

```json
{
  "GH": {
    "Token": "your-github-token",
    "RequestResponseLogging": false
  }
}
```

```csharp
using Soenneker.GitHub.Client.Http.Registrars;

services.AddGitHubHttpClientAsSingleton();
```

Keep the token in secret storage. Enable request/response logging only for controlled diagnostics: the authorization header is redacted, but URLs and bodies can still contain sensitive repository data.

## Use the client

```csharp
using Soenneker.GitHub.Client.Http.Abstract;

public sealed class GitHubRepositoryReader(IGitHubHttpClient clients)
{
    public async Task<HttpResponseMessage> Get(
        string owner,
        string repository,
        CancellationToken cancellationToken)
    {
        HttpClient client = await clients.Get(cancellationToken);
        return await client.GetAsync($"repos/{owner}/{repository}", cancellationToken);
    }
}
```

Callers borrow the returned client and must not dispose it. The provider removes its own cached client when disposed. Scoped providers use isolated cache entries, so disposing one scope cannot destroy another scope's transport.
