namespace DotNet.Libraries.Core.Lego.Requests.Environments;

/// <summary>
/// Represents the environment configuration for a request, including environment variables,
/// certificate folder, and certificate output format.
/// </summary>
/// <remarks>
/// <paramref name="CertificateFolder"/> is used as an output folder when a new certificate is requested or as a base
/// folder when a certificate is renewed. <paramref name="LetsEncryptEndpoint"/> can be used for configuring
/// a lego call to a route to a staging environment
/// </remarks>
public record RequestEnvironment(
	IReadOnlyDictionary<string, string?> Environment,
	string? CertificateFolder = null,
	string? CertificateFormat = null,
	string? LetsEncryptEndpoint = null);
