using System;
using System.Collections.Generic;
using System.Text;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Specifications
{
	class UserWithRelatedObjects : Specification<User>
	{
		public UserWithRelatedObjects()
		{
			Query = Source()
				.Fetch(x => x.Characters)
				.ThenFetch(x => x.Items)
				.ThenFetch(x => x.Runes)
				.ThenFetch(x => x.Rune);
		}
	}
}
