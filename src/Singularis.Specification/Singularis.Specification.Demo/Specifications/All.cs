using Singularis.Specification.Definition;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Specifications
{
	class All : Specification<User>
	{
		public All()
		{
			Query = Source();
		}
	}
}