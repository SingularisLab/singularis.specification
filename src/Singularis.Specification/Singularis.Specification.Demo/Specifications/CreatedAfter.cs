using Singularis.Specification.Definition;
using System;
using System.Collections.Generic;
using System.Text;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Specifications
{
	class CreatedAfter:Specification<Character>
	{
		public CreatedAfter(DateTime date)
		{
			Query = Source().Where(x => x.CreatedAt >= date);
		}
	}
}
