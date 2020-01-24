using Singularis.Specification.Definition;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Shared
{
	class WithTake : Specification<User>
	{
		public WithTake()
		{
			Query = Source().Take(5);
		}
	}
}