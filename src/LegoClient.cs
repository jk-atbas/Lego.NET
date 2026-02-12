using DotNet.Libraries.Core.Lego.Executors;
using DotNet.Libraries.Core.Lego.Requests;
using DotNet.Libraries.Core.Lego.Requests.Environments;
using DotNet.Libraries.Core.Lego.Results;
using DotNet.Libraries.Core.Lego.Secrets;
using Microsoft.Extensions.Logging;
using static DotNet.Libraries.Core.Lego.Helpers.LegoHelpers;

namespace DotNet.Libraries.Core.Lego;

public sealed class LegoClient(
	ILegoExecutor executor,
	ILogger<LegoClient>? logger = null)
{
	/// <summary>
	/// Initiates the process to acquire a new certificate using the specified request and secret source.
	/// </summary>
	/// <param name="request">
	/// The <see cref="ILegoRequest"/> containing the details required for the certificate acquisition process, 
	/// such as email address and domain names.
	/// </param>
	/// <param name="secretSource">
	/// An optional <see cref="ISecretSource"/> for retrieving secrets needed during the certificate acquisition process.
	/// </param>
	/// <param name="cancellationToken">
	/// A <see cref="CancellationToken"/> to observe while waiting for the task to complete,
	/// allowing the operation to be canceled.
	/// </param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> representing the asynchronous operation,
	/// with a result of type <see cref="LegoExecutionResult"/>.
	/// The result contains the success status, output logs, and, if successful, the path to the acquired certificate.
	/// </returns>
	/// <exception cref="Exception">
	/// Thrown if an error occurs during the certificate acquisition process.
	/// </exception>
	public async Task<LegoExecutionResult> AcquireCertificate(
		ILegoRequest request,
		ISecretSource? secretSource,
		CancellationToken cancellationToken)
	{
		logger?.BeginScope("Lego-New Certificate");
		logger?.LogInformation("Beginning certificate acquire process");

		try
		{
			return await BaseExecutorCall(request, secretSource, RequestType.New, cancellationToken);
		}
		catch (Exception e)
		{
			logger?.LogError(e, "Error while requesting new certificate");

			return new LegoExecutionResult(false, []);
		}
	}

	/// <summary>
	/// Renews an existing certificate using the specified request, secret source, and cancellation token.
	/// </summary>
	/// <param name="request">
	/// The <see cref="ILegoRequest"/> containing the details of the certificate renewal request, 
	/// such as email address and domain names.
	/// </param>
	/// <param name="secretSource">
	/// An optional <see cref="ISecretSource"/> for retrieving secrets required during the renewal process.
	/// </param>
	/// <param name="cancellationToken">
	/// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
	/// </param>
	/// <returns>
	/// A <see cref="LegoExecutionResult"/> representing the outcome of the certificate renewal process, 
	/// including its success status, output logs, and an optional certificate path.
	/// </returns>
	/// <exception cref="Exception">
	/// Thrown if an error occurs during the certificate renewal process.
	/// </exception>
	public async Task<LegoExecutionResult> RenewCertificate(
		ILegoRequest request,
		ISecretSource? secretSource,
		CancellationToken cancellationToken)
	{
		logger?.BeginScope("Lego-Renew Certificate");
		logger?.LogInformation("Beginning certificate renewal process");

		try
		{
			return await BaseExecutorCall(request, secretSource, RequestType.Renew, cancellationToken);
		}
		catch (Exception e)
		{
			logger?.LogError(e, "Error while renewing certificate was encountered");

			return new LegoExecutionResult(false, []);
		}
	}

	private async Task<LegoExecutionResult> BaseExecutorCall(
		ILegoRequest request,
		ISecretSource? secretSource,
		RequestType requestType,
		CancellationToken cancellationToken)
	{
		RequestEnvironment requestEnvironment = await request.BuildRequestEnvironment(secretSource, cancellationToken);

		var environment = new CommandEnvironment(
			CreateArgsList(request, requestEnvironment, requestType),
			requestEnvironment.Environment,
			requestEnvironment.CertificateFolder);

		return await executor.ExecuteAsync(environment, cancellationToken);
	}
}
