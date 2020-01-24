using Microsoft.EntityFrameworkCore.Internal;
using Singularis.Specification.Definition;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Specifications
{
	class WithItems : Specification<Character>
	{
		public WithItems()
		{
			Query = Source().Where(x => x.Items.Any());
		}
	}
}