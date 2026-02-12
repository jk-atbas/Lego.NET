namespace DotNet.Libraries.Core.Lego.Requests.Environments;

/// <summary>
/// Represents the environment in which a command is executed,
/// its arguments, and the associated environment variables.
/// </summary>
public record CommandEnvironment(
	IReadOnlyCollection<string> Arguments,
	IReadOnlyDictionary<string, string?> Environment,
	string? OutputPath = null,
	string? CertificateFormat = null);
