using DotNet.Libraries.Core.Lego.Providers;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace DotNet.Libraries.Core.Lego;

public sealed class LegoClient(
	IFileSystem fileSystem,
	IEnvironmentProvider environment,
	ILegoManifestProvider manifestProvider,
	ILogger<LegoClient>? logger = null)
{

}
