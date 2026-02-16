using DotNet.Libraries.Core.Lego.Enums;
using DotNet.Libraries.Core.Lego.Settings;
using DotNet.Libraries.Core.Lego.Tests.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO.Abstractions.TestingHelpers;

namespace DotNet.Libraries.Core.Lego.Tests.UnitTests.ConfigBinding;

public class ManifestBindingTests
{
	[Test]
	public void ShouldBindCorrectly()
	{
		const string manifestFileName = "settings.json";

		var settings = BuildDefaultCase(manifestFileName, JsonManifests.X64Windows);
		Assert.That(settings.Platforms, Is.Not.Empty);
	}

	[Test]
	public void ShouldBindMultipleOsesCorrectly()
	{
		const string fullyFilledManifest = "settings.json";

		var settings = BuildDefaultCase(fullyFilledManifest, JsonManifests.X64WindowsLinux);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(settings.Platforms, Is.Not.Empty);
			Assert.That(settings.Platforms, Has.Count.EqualTo(2));
			Assert.That(settings.Platforms.ContainsKey(LegoPlatform.Windows), Is.True);
			Assert.That(settings.Platforms.ContainsKey(LegoPlatform.Linux), Is.True);
		}
	}

	[Test]
	public void ShouldNotAllowEmptySettings()
	{
		const string fileName = "emptySettings.json";

		Assert
			.Throws<OptionsValidationException>(() => BuildDefaultCase(fileName, JsonManifests.EmptyWithOses));
	}

	[Test]
	public void ShouldNotAllowFullyEmptySettings()
	{
		const string fileName = "fullEmptySettings.json";

		Assert
			.Throws<OptionsValidationException>(() => BuildDefaultCase(fileName, JsonManifests.Empty));
	}

	private static LegoManifestSettings BuildDefaultCase(string fileName, string fileContent)
	{
		var fs = new MockFileSystem(new Dictionary<string, MockFileData>
		{
			{ fileName, new MockFileData(fileContent) },
		});

		var config = new ConfigurationBuilder()
			.AddJsonStream(fs.FileStream.New(fileName, FileMode.Open))
			.Build();

		var services = new ServiceCollection()
			.AddDefaultLegoClient(config, fs)
			.BuildServiceProvider();

		return services.GetRequiredService<IOptions<LegoManifestSettings>>().Value;
	}
}
