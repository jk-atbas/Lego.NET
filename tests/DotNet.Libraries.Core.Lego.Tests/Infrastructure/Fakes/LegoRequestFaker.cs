using Bogus;
using DotNet.Libraries.Core.Lego.Tests.Infrastructure.Requests;

namespace DotNet.Libraries.Core.Lego.Tests.Infrastructure.Fakes;

internal sealed class LegoRequestFaker : Faker<TestLegoRequest>
{
	public LegoRequestFaker()
	{
		RuleFor(
			p => p.EmailAddress,
			f => f.Internet.Email(
				provider: f.Internet.DomainName(),
				uniqueSuffix: Guid.NewGuid().ToString("N")));

		RuleFor(
			p => p.DomainNames,
			f => [f.Internet.DomainName(), f.Internet.DomainName()]);
	}
}
