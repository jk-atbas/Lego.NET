using DotNet.Libraries.Core.Lego.Elements;
using DotNet.Libraries.Core.Lego.Enums;
using Microsoft.Extensions.Options;

namespace DotNet.Libraries.Core.Lego.Settings;

/// <summary>
/// Represents the settings for the Lego manifest, including platform-specific configurations.
/// Intended for configuration-binding with <see cref="IOptions{TOptions}"/>
/// </summary>
public class LegoManifestSettings
{
	public Dictionary<LegoPlatform, LegoManifestEntry[]> Platforms { get; set; } = [];
}
