using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace DotNet.Libraries.Core.Lego.Elements;

/// <summary>
/// Represents an entry in the Lego manifest, containing details about the architecture, version, and executable path.
/// </summary>
public sealed class LegoManifestEntry
{
	[ConfigurationKeyName("arch")]
	public Architecture Architecture { get; set; }

	public Version Version { get; set; } = new(1, 0, 0);

	[ConfigurationKeyName("path")]
	public string ExecutablePath { get; set; } = string.Empty;
}
