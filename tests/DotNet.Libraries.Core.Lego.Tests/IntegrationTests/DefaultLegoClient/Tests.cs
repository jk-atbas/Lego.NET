using DotNet.Libraries.Core.Lego.Results;
using DotNet.Libraries.Core.Lego.Tests.Infrastructure.Containers.Acme;
using DotNet.Libraries.Core.Lego.Tests.Infrastructure.Fakes;
using DotNet.Libraries.Core.Lego.Tests.Infrastructure.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace DotNet.Libraries.Core.Lego.Tests.IntegrationTests.DefaultLegoClient;

[TestFixture]
public class Tests
{
	[Test]
	public void Success()
	{
		Assert.DoesNotThrowAsync(async () => await BaseTest(
			null,
			(dirInfo, fs, result) =>
			{
				using (Assert.EnterMultipleScope())
				{
					Assert.That(result.Success, Is.True);
					Assert.That(fs.Path.Exists(result.CertificatePath), Is.True);

					IFileInfo[] allDirFiles = dirInfo.EnumerateFiles("", SearchOption.AllDirectories).ToArray();

					Assert.That(
						allDirFiles.Any(f => f.FullName.Contains("issuer.crt", StringComparison.OrdinalIgnoreCase)),
						Is.True);

					Assert.That(
						allDirFiles.Any(f => f.FullName.Contains(".key", StringComparison.OrdinalIgnoreCase)),
						Is.True);
				}
			}));
	}

	[Test]
	public void SuccessWithPemCertFormat()
	{
		const string format = "pem";

		Assert.DoesNotThrowAsync(async () => await BaseTest(
			format,
			(dirInfo, fs, result) =>
			{
				using (Assert.EnterMultipleScope())
				{
					Assert.That(result.Success, Is.True);
					Assert.That(result.CertificatePath, Is.Not.Null);
					Assert.That(result.CertificatePath, Does.Contain(format));
					Assert.That(fs.File.Exists(result.CertificatePath), Is.True);

					using X509Certificate2? cert = ParseCert(result.CertificatePath!, format);
					Assert.That(cert, Is.Not.Null);

					var san = cert?.Extensions.OfType<X509SubjectAlternativeNameExtension>().FirstOrDefault();
					Assert.That(san, Is.Not.Null);
					Assert.That(san?.Format(false), Does.Contain("example.test"));
				}
			}));
	}

	[Test]
	public void SuccessWithPfxCertFormat()
	{
		const string format = "pfx";
		const string pw = "test9876";

		Assert.DoesNotThrowAsync(async () => await BaseTest(
			format,
			(dirInfo, fs, result) =>
			{
				using (Assert.EnterMultipleScope())
				{
					Assert.That(result.Success, Is.True);
					Assert.That(result.CertificatePath, Is.Not.Null);
					Assert.That(result.CertificatePath, Does.Contain(format));
					Assert.That(fs.File.Exists(result.CertificatePath), Is.True);

					using X509Certificate2? cert = ParseCert(result.CertificatePath!, format, pw);
					Assert.That(cert, Is.Not.Null);

					var san = cert?.Extensions.OfType<X509SubjectAlternativeNameExtension>().FirstOrDefault();
					Assert.That(san, Is.Not.Null);
					Assert.That(san?.Format(false), Does.Contain("example.test"));
				}
			},
			certPw: pw));
	}

	[Test]
	public void SuccessWithMultiSan()
	{
		const string format = "pfx";
		const string exampleDomain = "example.test";
		const string pw = "test1234";

		Assert.DoesNotThrowAsync(async () => await BaseTest(
			format,
			(dirInfo, fs, result) =>
			{
				using (Assert.EnterMultipleScope())
				{
					Assert.That(result.Success, Is.True);
					Assert.That(result.CertificatePath, Is.Not.Null);
					Assert.That(result.CertificatePath, Does.Contain(format));
					Assert.That(fs.File.Exists(result.CertificatePath), Is.True);

					using X509Certificate2? cert = ParseCert(result.CertificatePath!, format, pw);
					Assert.That(cert, Is.Not.Null);

					var san = cert?.Extensions.OfType<X509SubjectAlternativeNameExtension>().FirstOrDefault();
					Assert.That(san, Is.Not.Null);

					string[] dnsNames = san?.Format(false).Split(
						',',
						StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];

					Assert.That(dnsNames, Has.Length.EqualTo(2));
					Assert.That(dnsNames.All(s => s.Contains(exampleDomain, StringComparison.OrdinalIgnoreCase)), Is.True);
				}
			},
			[exampleDomain, $"www.{exampleDomain}"],
			pw));
	}

	private static async Task BaseTest(
		string? certFormat,
		Action<IDirectoryInfo, IFileSystem, LegoExecutionResult> assertions,
		string[]? domains = null,
		string? certPw = null)
	{
		AcmeEnvironment env = AcmeEnvironmentFixture.Env;
		var fs = new FileSystem();

		TestLegoRequest request = new LegoRequestFaker().Generate();
		request.LegoServerUrl = env.PebbleDirectoryUrl.ToString();
		request.LegoCaCertificates = fs.Path.Combine(
			AppContext.BaseDirectory,
			"IntegrationTests",
			"Assets",
			"pebble.minica.pem");

		if (domains is not null)
		{
			request.DomainNames = domains;
		}

		SetExecPath(request, fs);
		request.ChallTestSrvUrl = env.ChallengeTestSrvManagementUrl.ToString();
		request.CertFormat = certFormat;
		request.CertPw = certPw;

		using var _ = fs.CreateDisposableDirectory(request.GetTempDir(), out IDirectoryInfo directoryInfo);

		var config = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("native/manifest.json", false, false)
			.Build();

		var services = new ServiceCollection()
			.AddLogging()
			.AddDefaultLegoClient(config, fs)
			.BuildServiceProvider();

		var legoClient = services.GetRequiredService<LegoClient>();
		var result = await legoClient.AcquireCertificate(request, null, CancellationToken.None);

		assertions.Invoke(directoryInfo, fs, result);
	}

	private static X509Certificate2? ParseCert(string certPath, string certFormat = "crt", string certPw = "changeit")
	{
		return certFormat.Equals("pem", StringComparison.OrdinalIgnoreCase)
			   || certFormat.Equals("crt", StringComparison.OrdinalIgnoreCase)
			? X509CertificateLoader.LoadCertificateFromFile(certPath)
			: certFormat.Equals("pfx")
				? X509CertificateLoader.LoadPkcs12FromFile(certPath, certPw)
				: null;
	}

	private static void SetExecPath(TestLegoRequest request, FileSystem fs)
	{
		string execPath = fs.Path.Combine(AppContext.BaseDirectory, "DnsExecHelperTool");
		request.ExecPath = execPath;

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return;
		}

		fs.File.SetUnixFileMode(
			execPath,
			UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute
			| UnixFileMode.GroupRead | UnixFileMode.GroupExecute | UnixFileMode.OtherRead | UnixFileMode.OtherExecute);
	}
}
