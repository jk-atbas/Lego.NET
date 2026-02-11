namespace DotNet.Libraries.Core.Lego.Commands;

/// <summary>
/// Represents the environment in which a command is executed, including the command path, 
/// its arguments, and the associated environment variables.
/// </summary>
/// <remarks>
/// <paramref name="CertificateFolder"/> is used as an output folder when a new certificate is requested or as a base
/// folder when a certificate is renewed
/// </remarks>
public record CommandEnvironment(
	string CommandPath,
	IReadOnlyList<string> Arguments,
	IReadOnlyDictionary<string, string?> Environment,
	string? CertificateFolder = null,
	string? CertificateOutputFormat = null);
