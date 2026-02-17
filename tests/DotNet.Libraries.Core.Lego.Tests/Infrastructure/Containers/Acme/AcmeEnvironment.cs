using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

namespace DotNet.Libraries.Core.Lego.Tests.Infrastructure.Containers.Acme;

internal sealed class AcmeEnvironment : IAsyncDisposable
{
	private INetwork? network;
	private IContainer? challengeContainer;
	private IContainer? pebbleContainer;

	public Uri PebbleDirectoryUrl { get; private set; } = null!;
	public Uri ChallengeTestSrvManagementUrl { get; private set; } = null!;

	public async Task StartAsync(CancellationToken cancellationToken = default)
	{
		network = new NetworkBuilder()
			.WithName($"acme-it-{Guid.NewGuid():N}")
			.Build();

		await network.CreateAsync(cancellationToken);

		const string challtestsrvAlias = "challtestsrv";
		const int challtestsrvDnsPort = 8053;

		// challtestsrv
		challengeContainer = new ContainerBuilder("ghcr.io/letsencrypt/pebble-challtestsrv:latest")
			.WithNetwork(network)
			.WithNetworkAliases(challtestsrvAlias)
			// Disable default IPv6 to avoid ::1/AAAA surprises later
			.WithCommand(
				"-defaultIPv6", "",
				"-defaultIPv4", "10.0.0.10",
				"-doh", "")
			.WithPortBinding(8055, true) // Management-Port
			.WithPortBinding(challtestsrvDnsPort, true) // dns
			.WithPortBinding(5001, true) // tls-alpn-01
			.WithPortBinding(5002, true) // http-01
			.WithPortBinding(5003, true) // https http-01
			.WithWaitStrategy(Wait.ForUnixContainer().UntilExternalTcpPortIsAvailable(8055))
			.Build();

		await challengeContainer.StartAsync(cancellationToken);

		ushort challengeContainerManagementPort = challengeContainer.GetMappedPublicPort(8055);
		ChallengeTestSrvManagementUrl = new Uri($"http://localhost:{challengeContainerManagementPort}");

		// Pebble
		pebbleContainer = new ContainerBuilder("ghcr.io/letsencrypt/pebble:latest")
			.WithNetwork(network)
			.WithNetworkAliases("pebble")
			.WithCommand(
				"-config", "test/config/pebble-config.json",
				"-strict",
				"-dnsserver", $"{challtestsrvAlias}:{challtestsrvDnsPort}")
			.WithEnvironment("PEBBLE_VA_NOSLEEP", "1")
			.WithEnvironment("PEBBLE_WFE_NONCEREJECT", "0")
			.WithPortBinding(14000, true) // acme
			.WithPortBinding(15000, true) // management
			.WithWaitStrategy(Wait.ForUnixContainer().UntilExternalTcpPortIsAvailable(14000))
			.Build();

		await pebbleContainer.StartAsync(cancellationToken);

		ushort acmePort = pebbleContainer.GetMappedPublicPort(14000);
		PebbleDirectoryUrl = new Uri($"https://localhost:{acmePort}/dir");
	}

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		if (pebbleContainer is not null)
		{
			await pebbleContainer.DisposeAsync();
		}

		if (challengeContainer is not null)
		{
			await challengeContainer.DisposeAsync();
		}

		if (network is not null)
		{
			await network.DisposeAsync();
		}
	}
}
