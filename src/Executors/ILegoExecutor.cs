using DotNet.Libraries.Core.Lego.Commands;
using DotNet.Libraries.Core.Lego.Results;

namespace DotNet.Libraries.Core.Lego.Executors;

/// <summary>
/// Defines the contract for executing Lego commands within a specified environment 
/// and obtaining the results of the execution.
/// </summary>
public interface ILegoExecutor
{
	/// <summary>
	/// Executes a Lego command within the specified environment and returns the result of the execution.
	/// </summary>
	/// <param name="commandEnvironment">
	/// The environment in which the command is executed, including the command path, 
	/// its arguments, and the associated environment variables.
	/// </param>
	/// <param name="cancellationToken">
	/// A token to monitor for cancellation requests.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains 
	/// the <see cref="LegoExecutionResult"/> which includes the success status, output logs, 
	/// and an optional certificate path.
	/// </returns>
	Task<LegoExecutionResult> ExecuteAsync(CommandEnvironment commandEnvironment, CancellationToken cancellationToken);
}
