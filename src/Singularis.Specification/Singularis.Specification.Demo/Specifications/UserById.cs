using Singularis.Specification.Definition;
using Singularis.Specification.Demo.Models;

namespace Singularis.Specification.Demo.Specifications
{
	class UserById : Specification<User>
	{
		public UserById(int id)
		{
			Query = Source().Where(x => x.Id == id);
		}
	}
}