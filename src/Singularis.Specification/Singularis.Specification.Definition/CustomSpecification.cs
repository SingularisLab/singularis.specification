using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition
{
	class CustomSpecification<T> : Specification<T>
	{
		public CustomSpecification(IQuery query)
		{
			Query = query;
		}
	}
}