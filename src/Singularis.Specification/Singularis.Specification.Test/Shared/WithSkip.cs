using Singularis.Specification.Definition;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Shared
{
	class WithSkip : Specification<User>
	{
		public WithSkip()
		{
			Query = Source().Skip(5);
		}
	}
}