using DotNet.Libraries.Core.Lego.Settings;

namespace DotNet.Libraries.Core.Lego.Providers;

/// <summary>
/// Contract for a producer of Lego manifest infos
/// </summary>
public interface ILegoManifestProvider
{
	/// <summary>
	/// Returns the current <see cref="LegoManifestSettings"/>
	/// </summary>
	/// <returns>A current iteration of <seealso cref="LegoManifestSettings"/></returns>
	LegoManifestSettings ManifestSettings { get; }
}
