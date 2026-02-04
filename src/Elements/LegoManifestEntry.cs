using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DotNet.Libraries.Core.Lego.Elements;

/// <summary>
/// Represents an entry in the Lego manifest, containing details about the architecture, version, and executable path.
/// </summary>
public sealed class LegoManifestEntry(
	Architecture architecture,
	Version? version,
	string path)
{
	[JsonPropertyName("arch")]
	public Architecture Architecture { get; set; } = architecture;

	public Version Version { get; set; } = version ?? new Version(1, 0, 0);

	[JsonPropertyName("path")]
	public string ExecutablePath { get; set; } = path;
}
