using DotNet.Libraries.Core.Lego.Requests;
using DotNet.Libraries.Core.Lego.Requests.Environments;

namespace DotNet.Libraries.Core.Lego.Helpers;

internal static class LegoHelpers
{
	internal enum RequestType
	{
		New,
		Renew,
		Revoke,
	}

	/// <summary>
	/// Creates a list of arguments for executing a Lego command based on the provided request,
	/// environment configuration, and request type.
	/// </summary>
	/// <param name="request">
	/// The Lego request containing details such as email address and domain names.
	/// </param>
	/// <param name="environment">
	/// The environment configuration for the request, including certificate folder and output format.
	/// </param>
	/// <param name="requestType">
	/// The type of the request, indicating whether it is a new request or a renewal.
	/// </param>
	/// <returns>
	/// A read-only collection of strings representing the arguments for the Lego command.
	/// </returns>
	public static IReadOnlyCollection<string> CreateArgsList(
		ILegoRequest request,
		RequestEnvironment environment,
		RequestType requestType)
	{
		bool isDnsChallenge = request is IDnsChallengeRequest;

		var argsList = new List<string>
		{
			"--email", request.EmailAddress,
			"--accept-tos",
		};

		const string domainKey = "--domains";

		foreach (string domainName in request.DomainNames)
		{
			argsList.Add(domainKey);
			argsList.Add(domainName);
		}

		if (!string.IsNullOrWhiteSpace(environment.CertificateFolder))
		{
			argsList.Add("--path");
			argsList.Add(environment.CertificateFolder);
		}

		if (isDnsChallenge)
		{
			argsList.Add("--dns");
			argsList.Add(((IDnsChallengeRequest) request).DnsName);
		}

		if (!string.IsNullOrWhiteSpace(environment.CertificateFormat)
			&& (environment.CertificateFormat.Contains("pem", StringComparison.OrdinalIgnoreCase) ||
				environment.CertificateFormat.Contains("pfx", StringComparison.OrdinalIgnoreCase)))
		{
			argsList.Add($"--{environment.CertificateFormat}");
		}

		// Must be last argument
		var legoCommand = requestType switch
		{
			RequestType.New => "run",
			RequestType.Renew => "renew",
			RequestType.Revoke => "revoke",
			_ => string.Empty,
		};

		argsList.Add(legoCommand);

		return argsList;
	}
}
