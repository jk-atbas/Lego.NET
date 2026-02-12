using System.Runtime.InteropServices;

namespace DotNet.Libraries.Core.Lego.Enums.Extensions;

public static class EnumExtensions
{
	extension(LegoPlatform platform)
	{
		/// <summary>
		/// Maps a <see cref="LegoPlatform"/> value to an <see cref="OSPlatform"/>
		/// </summary>
		/// <returns>The corresponding <see cref="OSPlatform"/></returns>
		public OSPlatform ConvertToOsPlatform()
		{
			return platform switch
			{
				LegoPlatform.Windows => OSPlatform.Windows,
				LegoPlatform.Linux => OSPlatform.Linux,
				_ => default,
			};
		}
	}

	extension(OSPlatform platform)
	{
		/// <summary>
		/// Maps a <see cref="OSPlatform"/> value to a <see cref="LegoPlatform"/>
		/// </summary>
		/// <returns>The corresponding <see cref="LegoPlatform"/></returns>
		public LegoPlatform ConvertToLegoPlatform()
		{
			return platform.Equals(OSPlatform.Linux)
				? LegoPlatform.Linux
				: platform.Equals(OSPlatform.Windows)
					? LegoPlatform.Windows
					: default;
		}
	}
}
