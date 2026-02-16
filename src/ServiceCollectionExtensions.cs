using DotNet.Libraries.Core.Lego.Executors;
using DotNet.Libraries.Core.Lego.Providers;
using DotNet.Libraries.Core.Lego.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

namespace DotNet.Libraries.Core.Lego;

public static class ServiceCollectionExtensions
{
	extension(IServiceCollection serviceCollection)
	{
		/// <summary>
		/// Adds the default Lego client and its dependencies to the specified <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="legoManifestProviderSettings">
		/// The configuration settings for the Lego manifest provider bound to <see cref="LegoManifestSettings"/>.
		/// </param>
		/// <param name="fileSystem">
		/// An optional implementation of <see cref="IFileSystem"/>.
		/// If not provided, a default <see cref="FileSystem"/> instance is used.
		/// </param>
		/// <param name="environment">
		/// An optional implementation of <see cref="IEnvironmentProvider"/>.
		/// If not provided, the default <see cref="SystemEnvironmentProvider.Instance"/> is used.
		/// </param>
		/// <returns>
		/// The updated <see cref="IServiceCollection"/> with the default Lego client and its dependencies registered.
		/// </returns>
		public IServiceCollection AddDefaultLegoClient(
			IConfiguration legoManifestProviderSettings,
			IFileSystem? fileSystem = null,
			IEnvironmentProvider? environment = null)
		{
			fileSystem ??= new FileSystem();
			environment ??= SystemEnvironmentProvider.Instance;

			serviceCollection
				.AddOptions<LegoManifestSettings>()
				.Bind(legoManifestProviderSettings)
				.ValidateDataAnnotations()
				.Validate(settings => settings.Platforms.Count > 0, "At least one platform must be configured")
				.ValidateOnStart();

			serviceCollection
				.AddSingleton(fileSystem)
				.AddSingleton(environment)
				.AddSingleton<ILegoManifestProvider, DefaultLegoManifestProvider>()
				.AddSingleton<ILegoExecutor, DefaultExecutor>()
				.AddSingleton<LegoClient>();

			return serviceCollection;
		}
	}
}
