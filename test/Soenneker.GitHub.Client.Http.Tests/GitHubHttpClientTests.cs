using System.Net.Http;
using Soenneker.GitHub.Client.Http.Abstract;
using Soenneker.Tests.FixturedUnit;
using System.Threading.Tasks;
using Xunit;
using Soenneker.Extensions.HttpClient;
using Soenneker.Facts.Manual;
using FluentAssertions;

namespace Soenneker.GitHub.Client.Http.Tests;

[Collection("Collection")]
public class GitHubHttpClientTests : FixturedUnitTest
{
    private readonly IGitHubHttpClient _util;

    public GitHubHttpClientTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IGitHubHttpClient>(true);
    }

    [ManualFact]
    public async ValueTask Default()
    {
        HttpClient client = await _util.Get(CancellationToken);

        const string url = $"repos/dotnet/aspnetcore/discussions?per_page=100";

        string result = await client.SendToString(new HttpRequestMessage(HttpMethod.Get, url));
        result.Should().NotBeNullOrEmpty();
    }
}
