using DotNet.Libraries.Core.Lego.Requests;
using DotNet.Libraries.Core.Lego.Requests.Environments;
using DotNet.Libraries.Core.Lego.Secrets;

namespace DotNet.Libraries.Core.Lego.Tests.Infrastructure.Requests;

internal class TestLegoRequest : IDnsChallengeRequest
{
	public string EmailAddress { get; set; } = string.Empty;
	public string[] DomainNames { get; set; } = ["example.test"];
	public string DnsName => "exec";
	public string TestDir { get; set; } = string.Empty;
	public string LegoServerUrl { get; set; } = string.Empty;
	public string LegoCaCertificates { get; set; } = string.Empty;
	public string ExecPath { get; set; } = string.Empty;
	public string ChallTestSrvUrl { get; set; } = string.Empty;
	public string? CertFormat { get; set; }
	public string? CertPw { get; set; }

	public Task<RequestEnvironment> BuildRequestEnvironment(
		ISecretSource? secretSource,
		CancellationToken cancellationToken = default)
	{
		var envVars = new Dictionary<string, string?>
		{
			{ "LEGO_CA_CERTIFICATES", LegoCaCertificates },
			{ "EXEC_PATH", ExecPath },
			{ "CHALLTESTSRV_URL", ChallTestSrvUrl },
		};

		return Task.FromResult(new RequestEnvironment(
			envVars,
			GetTempDir(),
			CertFormat,
			LegoServerUrl,
			["--dns.propagation-disable-ans"],
			CertPw));
	}

	public string GetTempDir()
	{
		string dir = new(TestDir.AsSpan()[1..]);

		return Path.Combine(AppContext.BaseDirectory, dir);
	}
}
