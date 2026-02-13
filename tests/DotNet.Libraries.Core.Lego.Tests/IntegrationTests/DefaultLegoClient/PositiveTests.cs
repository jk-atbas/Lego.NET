using DotNet.Libraries.Core.Lego.Tests.Infrastructure.Fakes;

namespace DotNet.Libraries.Core.Lego.Tests.IntegrationTests.DefaultLegoClient;

internal class PositiveTests
{
	[Test]
	public async Task Success()
	{
		var request = new LegoRequestFaker().Generate();
	}
}
