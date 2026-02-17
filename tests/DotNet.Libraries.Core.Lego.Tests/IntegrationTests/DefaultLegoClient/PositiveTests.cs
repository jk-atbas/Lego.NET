using DotNet.Libraries.Core.Lego.Tests.Infrastructure.Containers.Acme;
using DotNet.Libraries.Core.Lego.Tests.Infrastructure.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

namespace DotNet.Libraries.Core.Lego.Tests.IntegrationTests.DefaultLegoClient;

[TestFixture]
public class PositiveTests
{
	[Test]
	public async Task Success()
	{
		AcmeEnvironment env = AcmeEnvironmentFixture.Env;
		var fs = new FileSystem();

		var request = new LegoRequestFaker().Generate();
		request.LegoServerUrl = env.PebbleDirectoryUrl.ToString();
		request.LegoCaCertificates = fs.Path.Combine(AppContext.BaseDirectory, "IntegrationTests", "Assets", "pebble.minica.pem");
		request.ExecPath = fs.Path.Combine(AppContext.BaseDirectory, "DnsExecHelperTool");
		request.ChallTestSrvUrl = env.ChallengeTestSrvManagementUrl.ToString();

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

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.Success, Is.True);
			Assert.That(fs.Path.Exists(result.CertificatePath), Is.True);
		}
	}
}
