using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DotNet.Libraries.Core.Lego.Secrets;

/// <summary>
/// Represents an encrypted secret containing the cipher text, initialization vector (IV), 
/// encryption algorithm, key identifier, and optional associated data.
/// </summary>
public record EncryptedSecret(
	byte[] CipherText,
	byte[] Iv,
	string Algorithm,
	string KeyId,
	byte[]? AssociatedData)
{
	/// <summary>
	/// Returns a string that represents a json hashed through MD-5 in hexadecimal form
	/// </summary>
	/// <returns>Hexadecimal string representation</returns>
	public override string ToString()
	{
		string jsonRepresentation = JsonSerializer.Serialize(this);
		byte[] representation = MD5.HashData(Encoding.UTF8.GetBytes(jsonRepresentation));

		return "Secret as a json representation as a MD-5 hash in hexadecimal form: "
			   + Convert.ToHexStringLower(representation);
	}
}
