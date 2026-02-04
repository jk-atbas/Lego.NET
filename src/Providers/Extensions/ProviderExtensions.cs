using DotNet.Libraries.Core.Lego.Elements;
using DotNet.Libraries.Core.Lego.Enums;
using System.Runtime.InteropServices;

namespace DotNet.Libraries.Core.Lego.Providers.Extensions;

public static class ProviderExtensions
{
	extension(ILegoManifestProvider provider)
	{
		/// <summary>
		/// Retrieves a <see cref="LegoManifestEntry"/> for the specified
		/// <paramref name="legoPlatform"/> and <paramref name="architecture"/>.
		/// </summary>
		/// <param name="legoPlatform">
		/// The <see cref="LegoPlatform"/> for which the manifest entry is requested.
		/// </param>
		/// <param name="architecture">
		/// The <see cref="Architecture"/> of the manifest entry. Defaults to <see cref="Architecture.X64"/>.
		/// </param>
		/// <returns>
		/// A <see cref="LegoManifestEntry"/> if a matching entry is found; otherwise, <c>null</c>.
		/// </returns>
		/// <exception cref="NotSupportedException">
		/// Thrown when the specified <paramref name="architecture"/>
		/// is not <see cref="Architecture.X64"/> or <see cref="Architecture.X86"/>.
		/// </exception>
		public LegoManifestEntry? GetLegoManifestEntry(
			LegoPlatform legoPlatform,
			Architecture architecture = Architecture.X64)
		{
			return architecture is not Architecture.X64 and not Architecture.X86
				? throw new NotSupportedException("Only x64 & x86 architectures are currently supported")
				: !provider.ManifestSettings.Platforms.TryGetValue(legoPlatform, out LegoManifestEntry[]? manifests)
					? null
					: manifests.FirstOrDefault(m => m.Architecture == architecture);
		}
	}
}
