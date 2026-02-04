using DotNet.Libraries.Core.Lego.Enums;
using DotNet.Libraries.Core.Lego.Providers.Extensions;
using DotNET.Libraries.Core.Lego.Tests.Infrastructure;

namespace DotNET.Libraries.Core.Lego.Tests.UnitTests.SettingsTests;

public class LegoManifestSettingsTests
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void EmptyCollectionShouldReturnNoManifestEntry()
	{
		var provider = new TestLegoSettingsProvider([]);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(provider.GetLegoManifestEntry(LegoPlatform.Linux), Is.Null);
			Assert.That(provider.GetLegoManifestEntry(LegoPlatform.Windows), Is.Null);
			Assert.That(provider.GetLegoManifestEntry(LegoPlatform.Unknown), Is.Null);
		}
	}
}
