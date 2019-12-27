using System;
using Singularis.Specification.Domain.Query;

namespace Singularis.Specification.Domain
{
	public class Specification: ISpecification
	{
		public IQuery Query { get; protected set; }
		public IQueryContext Context { get; protected set; }

		public Type ResultType => ((Query.Query)Query).Parent == null ? ((Query.Query)Query).EntityType : ((Query.Query)Query).Parent.Parameters.OutType;

		public Specification()
		{
			Context = new QueryContext();
		}

		protected IQuery<TSource> Source<TSource>()
		{
			return new Query<TSource>();
		}
	}

	public class Specification<T> : Specification, ISpecification<T>
	{
		protected IQuery<T> Source()
		{
			return Source<T>();
		}
	}
}