using Soenneker.GitHub.Client.Http.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.GitHub.Client.Http.Tests;

[Collection("Collection")]
public class GitHubHttpClientTests : FixturedUnitTest
{
    private readonly IGitHubHttpClient _util;

    public GitHubHttpClientTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IGitHubHttpClient>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
