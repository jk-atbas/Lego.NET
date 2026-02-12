namespace DotNet.Libraries.Core.Lego.Requests.Environments;

/// <summary>
/// Represents the environment configuration for a request, including environment variables,
/// certificate folder, and certificate output format.
/// </summary>
public record RequestEnvironment(
	IReadOnlyDictionary<string, string?> Environment,
	string? CertificateFolder = null,
	string? CertificateOutputFormat = null);
