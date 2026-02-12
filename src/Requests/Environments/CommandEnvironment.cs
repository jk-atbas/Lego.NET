namespace DotNet.Libraries.Core.Lego.Requests.Environments;

/// <summary>
/// Represents the environment in which a command is executed,
/// its arguments, and the associated environment variables.
/// </summary>
/// <remarks>
/// <paramref name="CertificateFolder"/> is used as an output folder when a new certificate is requested or as a base
/// folder when a certificate is renewed
/// </remarks>
public record CommandEnvironment(
	IReadOnlyCollection<string> Arguments,
	IReadOnlyDictionary<string, string?> Environment,
	string? CertificateFolder = null,
	string? CertificateOutputFormat = null);
