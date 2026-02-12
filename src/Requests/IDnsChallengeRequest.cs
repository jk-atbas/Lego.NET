using DotNet.Libraries.Core.Lego.Requests.Environments;
using DotNet.Libraries.Core.Lego.Secrets;

namespace DotNet.Libraries.Core.Lego.Requests;

/// <summary>
/// Represents a request for performing a DNS challenge in the Lego framework.
/// </summary>
/// <remarks>
/// This interface extends <see cref="ILegoRequest"/> to provide additional functionality
/// specific to DNS challenges, such as configuring the environment with DNS-related secrets.
/// </remarks>
public interface IDnsChallengeRequest : ILegoRequest
{
	/// <summary>
	/// The dns name for the Lego cli tool
	/// </summary>
	/// <remarks>See <see href="https://go-acme.github.io/lego/dns/index.html"/></remarks>
	string DnsName { get; }

	/// <summary>
	/// Configure the lego environment before attempting the DNS challenge
	/// </summary>
	/// <param name="secretSource">Origin of the needed DNS challenge secrets</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The configured <see cref="RequestEnvironment"/></returns>
	Task<RequestEnvironment> BuildCommandEnvironment(ISecretSource secretSource, CancellationToken cancellationToken);

	async Task<RequestEnvironment> ILegoRequest.BuildRequestEnvironment(
		CancellationToken cancellationToken,
		ISecretSource? source)
	{
		return source is null
			? throw new ArgumentNullException(
				nameof(source),
				"When an actual DNS challenge is needed the secret source must exist")
			: await BuildCommandEnvironment(source, cancellationToken);
	}
}
