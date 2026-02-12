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
}
