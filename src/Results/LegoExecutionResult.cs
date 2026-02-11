namespace DotNet.Libraries.Core.Lego.Results;

/// <summary>
/// Represents the result of a Lego execution process, including its success status, output logs, and an optional certificate path.
/// </summary>
/// <param name="Success">Whether the Lego request was successful</param>
/// <param name="OutputLogs">All outputs for the operation</param>
/// <param name="CertificatePath">If successful, a path to the certificate otherwise null</param>
public record LegoExecutionResult(
	bool Success,
	IList<string> OutputLogs,
	string? CertificatePath = null);
