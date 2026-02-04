using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace DotNet.Libraries.Core.Lego.Elements;

/// <summary>
/// Represents an entry in the Lego manifest, containing details about the architecture, version, and executable path.
/// </summary>
public readonly record struct LegoManifestEntry(
	[field: JsonPropertyName("arch")] Architecture Architecture,
	Version Version,
	[field: JsonPropertyName("path")] string ExecutablePath);
