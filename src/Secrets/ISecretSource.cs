namespace DotNet.Libraries.Core.Lego.Secrets;

/// <summary>
/// Represents a source for retrieving secrets in an asynchronous manner.
/// </summary>
public interface ISecretSource
{
	/// <summary>
	/// Retrieves a secret of the specified type.
	/// </summary>
	/// <typeparam name="TSecretOutput">The type of the secret to retrieve.</typeparam>
	/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the secret of type <typeparamref name="TSecretOutput"/>.
	/// </returns>
	Task<TSecretOutput> GetSecret<TSecretOutput>(CancellationToken cancellationToken);
}
