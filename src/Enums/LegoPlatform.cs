using System.Text.Json.Serialization;

namespace DotNet.Libraries.Core.Lego.Enums;

/// <summary>
/// Represents the supported platforms for the Lego library.
/// </summary>
public enum LegoPlatform
{
	/// <summary>
	/// The platform is unknown or not specified.
	/// </summary>
	Unknown,

	/// <summary>
	/// The platform is Microsoft Windows.
	/// </summary>
	[JsonPropertyName("win")]
	Windows,

	/// <summary>
	/// The platform is Linux.
	/// </summary>
	Linux,
}
