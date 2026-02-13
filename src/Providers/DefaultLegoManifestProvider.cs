using DotNet.Libraries.Core.Lego.Settings;
using Microsoft.Extensions.Options;

namespace DotNet.Libraries.Core.Lego.Providers;

/// <summary>
/// Provides the default implementation of <see cref="ILegoManifestProvider"/> 
/// to supply Lego manifest settings.
/// </summary>
/// <remarks>
/// This class retrieves and exposes the current Lego manifest settings
/// using an <see cref="IOptionsMonitor{TOptions}"/> for <see cref="LegoManifestSettings"/>.
/// </remarks>
public sealed class DefaultLegoManifestProvider(IOptionsMonitor<LegoManifestSettings> optionsMonitor)
	: ILegoManifestProvider
{
	/// <inheritdoc />
	public LegoManifestSettings ManifestSettings { get; } = optionsMonitor.CurrentValue;
}
