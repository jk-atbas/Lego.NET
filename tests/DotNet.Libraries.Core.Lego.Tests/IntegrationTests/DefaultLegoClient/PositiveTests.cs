using DotNet.Libraries.Core.Lego.Tests.Infrastructure.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

namespace DotNet.Libraries.Core.Lego.Tests.IntegrationTests.DefaultLegoClient;

internal class PositiveTests
{
	[Test]
	public async Task Success()
	{
		var request = new LegoRequestFaker().Generate();
		var fs = new FileSystem();
		using var _ = fs.CreateDisposableDirectory(request.GetTempDir(), out var directoryInfo);

		var config = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("native/manifest.json", false, false)
			.Build();

		var services = new ServiceCollection()
			.AddLogging()
			.AddDefaultLegoClient(config, fs)
			.BuildServiceProvider();

		var legoClient = services.GetRequiredService<LegoClient>();
		var result = await legoClient.AcquireCertificate(request, null, CancellationToken.None);

		Assert.That(result.Success, Is.True);
	}
}
