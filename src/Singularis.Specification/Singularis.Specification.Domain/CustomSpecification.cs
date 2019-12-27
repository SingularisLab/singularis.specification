using Singularis.Specification.Domain.Query;

namespace Singularis.Specification.Domain
{
	class CustomSpecification : Specification
	{
		public CustomSpecification(IQuery query, IQueryContext queryContext)
		{
			Query = query;
			Context = queryContext;
		}
	}

	class CustomSpecification<T> : Specification<T>
	{
		public CustomSpecification(IQuery query, IQueryContext queryContext)
		{
			Query = query;
			Context = queryContext;
		}
	}
}