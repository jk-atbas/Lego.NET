using DotNet.Libraries.Core.Lego.Tests.Infrastructure.Containers.Acme;

namespace DotNet.Libraries.Core.Lego.Tests.IntegrationTests;

[SetUpFixture]
public class AcmeEnvironmentFixture
{
	internal static AcmeEnvironment Env { get; private set; } = null!;

	[OneTimeSetUp]
	public async Task OneTimeSetup()
	{
		Env = new AcmeEnvironment();
		await Env.StartAsync();
	}

	[OneTimeTearDown]
	public async Task OneTimeTearDown()
	{
		await Env.DisposeAsync();
	}
}
