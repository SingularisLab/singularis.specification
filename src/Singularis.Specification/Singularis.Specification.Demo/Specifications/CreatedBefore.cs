using System;
using Singularis.Specification.Definition;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Specifications
{
	class CreatedBefore : Specification<Character>
	{
		public CreatedBefore(DateTime date)
		{
			Query = Source().Where(x => x.CreatedAt <= date);
		}
	}
}