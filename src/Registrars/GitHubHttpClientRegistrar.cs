using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.GitHub.Client.Http.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.GitHub.Client.Http.Registrars;

/// <summary>
/// A .NET thread-safe singleton HttpClient for GitHub
/// </summary>
public static class GitHubHttpClientRegistrar
{
    /// <summary>
    /// Adds <see cref="IGitHubHttpClient"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddGitHubHttpClientAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCache();
        services.TryAddSingleton<IGitHubHttpClient, GitHubHttpClient>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IGitHubHttpClient"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddGitHubHttpClientAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCache();
        services.TryAddScoped<IGitHubHttpClient, GitHubHttpClient>();

        return services;
    }
}