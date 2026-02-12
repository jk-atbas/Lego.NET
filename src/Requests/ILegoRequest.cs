using DotNet.Libraries.Core.Lego.Requests.Environments;
using DotNet.Libraries.Core.Lego.Secrets;

namespace DotNet.Libraries.Core.Lego.Requests;

/// <summary>
/// Represents a request in the Lego framework, providing the necessary data and methods
/// to configure and execute Lego commands.
/// </summary>
/// <remarks>
/// This interface serves as a base for specific types of Lego requests, such as DNS challenges.
/// It includes properties for essential request details like email address and domain names,
/// as well as a method to build the command environment.
/// </remarks>
public interface ILegoRequest
{
	/// <summary>
	/// Associated email address for the certificate request
	/// </summary>
	string EmailAddress { get; }

	/// <summary>
	/// Associated domain names for the certificate request
	/// </summary>
	string[] DomainNames { get; }

	/// <summary>
	/// Configure the environment before executing a Lego request
	/// </summary>
	/// <param name="secretSource">Optional secret source</param>
	/// <returns>The configured <see cref="RequestEnvironment"/></returns>
	Task<RequestEnvironment> BuildRequestEnvironment(CancellationToken cancellationToken, ISecretSource? secretSource = null);
}
