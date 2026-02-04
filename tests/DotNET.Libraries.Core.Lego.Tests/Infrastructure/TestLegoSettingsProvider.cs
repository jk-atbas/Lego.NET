using DotNet.Libraries.Core.Lego.Elements;
using DotNet.Libraries.Core.Lego.Enums;
using DotNet.Libraries.Core.Lego.Providers;
using DotNet.Libraries.Core.Lego.Settings;

namespace DotNet.Libraries.Core.Lego.Tests.Infrastructure;

internal sealed class TestLegoSettingsProvider(Dictionary<LegoPlatform, LegoManifestEntry[]> dict)
	: ILegoManifestProvider
{
	/// <inheritdoc />
	public LegoManifestSettings ManifestSettings { get; } = new LegoManifestSettings { Platforms = dict };
}
