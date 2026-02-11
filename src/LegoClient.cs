using DotNet.Libraries.Core.Lego.Executors;
using DotNet.Libraries.Core.Lego.Requests;
using DotNet.Libraries.Core.Lego.Results;
using Microsoft.Extensions.Logging;

namespace DotNet.Libraries.Core.Lego;

public sealed class LegoClient(
	ILegoExecutor executor,
	ILogger<LegoClient> logger)
{
	public async Task<LegoExecutionResult> AcquireCertificate(ILegoRequest request, CancellationToken cancellationToken)
	{
		return new LegoExecutionResult(false, []);
	}

	public async Task<LegoExecutionResult> RenewCertificate(ILegoRequest request, CancellationToken cancellationToken)
	{
		return new LegoExecutionResult(false, []);
	}
}
