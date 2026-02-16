using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace DotNet.Libraries.Core.Lego.Elements;

/// <summary>
/// Represents an entry in the Lego manifest, containing details about the architecture, version, and executable path.
/// </summary>
public sealed class LegoManifestEntry
{
	[ConfigurationKeyName("arch")]
	public required Architecture Architecture { get; init; }

	public required Version? Version { get; init; }

	[ConfigurationKeyName("path")]
	public required string ExecutablePath { get; init; }
}
