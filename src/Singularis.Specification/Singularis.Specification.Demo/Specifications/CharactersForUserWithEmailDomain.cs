using System.Linq;
using Singularis.Specification.Definition;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Specifications
{
	class CharactersForUserWithEmailDomain: Specification<ReadCharacter>
	{
		public CharactersForUserWithEmailDomain(string domain)
		{
			var userQuery = Source<User>().Where(x => x.Email.Contains(domain)).Projection(x => x.Id);
			Query = Source().Where((x, ctx) => ctx.GetQueryResult<int>(userQuery).Contains(x.UserId));
		}
	}
}