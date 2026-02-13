using DotNet.Libraries.Core.Lego.Requests;
using DotNet.Libraries.Core.Lego.Requests.Environments;
using DotNet.Libraries.Core.Lego.Secrets;

namespace DotNet.Libraries.Core.Lego.Tests.Infrastructure.Requests;

internal class TestLegoRequest : ILegoRequest
{
	public string EmailAddress { get; set; }
	public string[] DomainNames { get; set; }
	public Task<RequestEnvironment> BuildRequestEnvironment(ISecretSource? secretSource, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}
