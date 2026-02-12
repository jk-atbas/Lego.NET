namespace DotNet.Libraries.Core.Lego.Secrets;

/// <summary>
/// Represents a source for retrieving secrets in an asynchronous manner.
/// </summary>
public interface ISecretSource
{
	/// <summary>
	/// Asynchronously retrieves a secret as a byte array.
	/// </summary>
	/// <param name="cancellationToken">
	/// A <see cref="CancellationToken"/> that can be used to cancel the operation.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the secret as a byte array.
	/// </returns>
	Task<byte[]> GetSecretAsync(CancellationToken cancellationToken);
}
