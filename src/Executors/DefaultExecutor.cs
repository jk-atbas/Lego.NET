using CliWrap;
using CliWrap.Buffered;
using DotNet.Libraries.Core.Lego.Elements;
using DotNet.Libraries.Core.Lego.Enums;
using DotNet.Libraries.Core.Lego.Enums.Extensions;
using DotNet.Libraries.Core.Lego.Providers;
using DotNet.Libraries.Core.Lego.Providers.Extensions;
using DotNet.Libraries.Core.Lego.Requests.Environments;
using DotNet.Libraries.Core.Lego.Results;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;
using System.Runtime.InteropServices;

namespace DotNet.Libraries.Core.Lego.Executors;

internal sealed partial class DefaultExecutor(
	IFileSystem fileSystem,
	ILegoManifestProvider legoProvider,
	ILogger<DefaultExecutor>? logger = null) : ILegoExecutor
{
	private const string NoLegoRuntimeFound = "No compatible Lego runtimes were found for your configuration";

	private static readonly LegoPlatform osPlatform = CurrentOsPlatform().ConvertToLegoPlatform();
	private static readonly Architecture architecture = RuntimeInformation.OSArchitecture;

	/// <inheritdoc />
	public async Task<LegoExecutionResult> ExecuteAsync(
		CommandEnvironment commandEnvironment,
		CancellationToken cancellationToken)
	{
		logger?.BeginScope("Lego-{component}", nameof(DefaultExecutor));
		LogStartingExecutionForOsWithArchitecture(logger, osPlatform, architecture);
		LegoManifestEntry? legoManifest = legoProvider.GetLegoManifestEntry(osPlatform, architecture);

		if (legoManifest is null)
		{
			logger?.LogInformation(NoLegoRuntimeFound);

			return new LegoExecutionResult(false, [NoLegoRuntimeFound]);
		}

		try
		{
			BufferedCommandResult result = await Cli
				.Wrap(fileSystem.Path.Combine(AppContext.BaseDirectory, legoManifest.ExecutablePath))
				.WithArguments(commandEnvironment.Arguments)
				.WithEnvironmentVariables(commandEnvironment.Environment)
				.ExecuteBufferedAsync(cancellationToken);

			string? certPath = null;

			if (!result.IsSuccess || string.IsNullOrWhiteSpace(commandEnvironment.OutputPath))
			{
				return new LegoExecutionResult(
					result.IsSuccess,
					[result.StandardError, result.StandardOutput],
					certPath
				);
			}

			IDirectoryInfo certDir = fileSystem.DirectoryInfo.New(fileSystem.Path.Combine(
				commandEnvironment.OutputPath,
				"certificates"));

			if (certDir.Exists)
			{
				certPath = certDir.EnumerateFiles(
						string.IsNullOrWhiteSpace(commandEnvironment.CertificateFormat)
							? "*.crt"
							: commandEnvironment.CertificateFormat)
					.Select(f => f.FullName)
					.FirstOrDefault();
			}

			return new LegoExecutionResult(
				result.IsSuccess,
				[result.StandardError, result.StandardOutput],
				certPath
			);
		}
		catch (Exception e)
		{
			logger?.LogError(e, "Error while executing lego command");

			return new LegoExecutionResult(false, [e.Message, e.StackTrace ?? string.Empty]);
		}
	}

	private static OSPlatform CurrentOsPlatform()
	{
		return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
			? OSPlatform.Windows
			: RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
				? OSPlatform.Linux
				: throw new NotSupportedException("Only Windows and Linux is currently supported");
	}

	[LoggerMessage(LogLevel.Information, "Starting execution for os {os} with {arch} architecture")]
	static partial void LogStartingExecutionForOsWithArchitecture(
		ILogger<DefaultExecutor>? logger,
		LegoPlatform os,
		Architecture arch);
}
