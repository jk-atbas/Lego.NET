using DotNet.Libraries.Core.Lego.Requests;
using DotNet.Libraries.Core.Lego.Requests.Environments;
using DotNet.Libraries.Core.Lego.Secrets;
using System.Collections.Frozen;

namespace DotNet.Libraries.Core.Lego.Tests.Infrastructure.Requests;

internal class TestLegoRequest : ILegoRequest
{
	public string EmailAddress { get; set; } = string.Empty;
	public string[] DomainNames { get; set; } = [];
	public string TestDir { get; set; } = string.Empty;

	private const string LegoStagingServerUrl = "https://acme-staging-v02.api.letsencrypt.org/directory";

	public Task<RequestEnvironment> BuildRequestEnvironment(
		ISecretSource? secretSource,
		CancellationToken cancellationToken = default)
	{
		return Task.FromResult(new RequestEnvironment(
			new Dictionary<string, string?>().ToFrozenDictionary(),
			GetTempDir(),
			null,
			LegoStagingServerUrl));
	}

	public string GetTempDir()
	{
		string dir = new(TestDir.AsSpan()[1..]);

		return Path.Combine(AppContext.BaseDirectory, dir);
	}
}
