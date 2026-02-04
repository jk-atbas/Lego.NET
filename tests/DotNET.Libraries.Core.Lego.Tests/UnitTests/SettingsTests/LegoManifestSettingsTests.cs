using DotNet.Libraries.Core.Lego.Elements;
using DotNet.Libraries.Core.Lego.Enums;
using DotNet.Libraries.Core.Lego.Providers.Extensions;
using DotNet.Libraries.Core.Lego.Tests.Infrastructure;
using System.Runtime.InteropServices;

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
			Assert.That(provider.ManifestSettings.Platforms, Is.Empty);
			Assert.That(provider.GetLegoManifestEntry(LegoPlatform.Linux), Is.Null);
			Assert.That(provider.GetLegoManifestEntry(LegoPlatform.Windows), Is.Null);
			Assert.That(provider.GetLegoManifestEntry(LegoPlatform.Unknown), Is.Null);
		}
	}

	[Test]
	public void NonX86ArchShouldThrowNotSupportedException()
	{
		var provider = new TestLegoSettingsProvider([]);

		using (Assert.EnterMultipleScope())
		{
			Assert.Throws<NotSupportedException>(() =>
				provider.GetLegoManifestEntry(LegoPlatform.Linux, Architecture.Wasm));

			Assert.Throws<NotSupportedException>(() =>
				provider.GetLegoManifestEntry(LegoPlatform.Linux, Architecture.Arm64));

			Assert.Throws<NotSupportedException>(() =>
				provider.GetLegoManifestEntry(LegoPlatform.Windows, Architecture.LoongArch64));

			Assert.DoesNotThrow(() => provider.GetLegoManifestEntry(LegoPlatform.Windows, Architecture.X86));
		}
	}

	[Test]
	public void ShouldReturnX86Entry()
	{
		const string testExePath = "test.exe";
		var testVersion = new Version(1, 0, 0);
		var linuxX64 = new LegoManifestEntry(Architecture.X64, testVersion, "some path...");
		var linuxX86 = new LegoManifestEntry(Architecture.X86, testVersion, testExePath);

		var baseDict = new Dictionary<LegoPlatform, LegoManifestEntry[]>
		{
			{ LegoPlatform.Linux, [linuxX64, linuxX86] },
		};

		var provider = new TestLegoSettingsProvider(baseDict);

		using (Assert.EnterMultipleScope())
		{
			LegoManifestEntry? entry = provider.GetLegoManifestEntry(LegoPlatform.Linux, Architecture.X86);
			Assert.That(entry, Is.Not.Null);
			Assert.That(entry, Is.Not.Default);
			Assert.That(entry?.ExecutablePath, Is.EqualTo(testExePath));
		}
	}

	[Test]
	public void ShouldReturnFirstX86Entry()
	{
		const string testExePath = "test.exe";
		var testVersion = new Version(1, 0, 0);
		var linuxX86Alternate = new LegoManifestEntry(Architecture.X86, testVersion, "some path...");
		var linuxX86 = new LegoManifestEntry(Architecture.X86, testVersion, testExePath);

		var baseDict = new Dictionary<LegoPlatform, LegoManifestEntry[]>
		{
			{ LegoPlatform.Linux, [linuxX86, linuxX86Alternate] },
		};

		var provider = new TestLegoSettingsProvider(baseDict);

		using (Assert.EnterMultipleScope())
		{
			LegoManifestEntry? entry = provider.GetLegoManifestEntry(LegoPlatform.Linux, Architecture.X86);
			Assert.That(entry, Is.Not.Null);
			Assert.That(entry, Is.Not.Default);
			Assert.That(entry?.ExecutablePath, Is.EqualTo(testExePath));
		}
	}

	[Test]
	public void ShouldReturnNullForNotFoundArch()
	{
		var linuxX64 = new LegoManifestEntry(Architecture.X64, null, "important path");
		var baseDict = new Dictionary<LegoPlatform, LegoManifestEntry[]>
		{
			{ LegoPlatform.Linux, [linuxX64] }
		};

		var provider = new TestLegoSettingsProvider(baseDict);

		LegoManifestEntry? entry = provider.GetLegoManifestEntry(LegoPlatform.Linux, Architecture.X86);
		Assert.That(entry, Is.Null);
	}

	[Test]
	public void ShouldReturnNullForNotFoundPlatform()
	{
		var linuxX64 = new LegoManifestEntry(Architecture.X64, null, "important path");
		var baseDict = new Dictionary<LegoPlatform, LegoManifestEntry[]>
		{
			{ LegoPlatform.Linux, [linuxX64] }
		};

		var provider = new TestLegoSettingsProvider(baseDict);

		LegoManifestEntry? entry = provider.GetLegoManifestEntry(LegoPlatform.Windows);
		Assert.That(entry, Is.Null);
	}
}
