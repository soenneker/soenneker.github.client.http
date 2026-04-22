using System.Net.Http;
using Soenneker.GitHub.Client.Http.Abstract;
using Soenneker.Tests.HostedUnit;
using System.Threading.Tasks;
using Soenneker.Extensions.HttpClient;
using Soenneker.Facts.Manual;
using AwesomeAssertions;

namespace Soenneker.GitHub.Client.Http.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class GitHubHttpClientTests : HostedUnitTest
{
    private readonly IGitHubHttpClient _util;

    public GitHubHttpClientTests(Host host) : base(host)
    {
        _util = Resolve<IGitHubHttpClient>(true);
    }

    [Test]
    public async ValueTask Get_should_get()
    {
        HttpClient client = await _util.Get(CancellationToken);
        client.Should().NotBeNull();
    }

    [ManualFact]
    public async ValueTask Send_Test()
    {
        HttpClient client = await _util.Get(CancellationToken);

        const string url = $"repos/dotnet/aspnetcore/discussions?per_page=100";

        string result = await client.SendToString(new HttpRequestMessage(HttpMethod.Get, url));
        result.Should().NotBeNullOrEmpty();
    }
}
