using System;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition
{
	public class Specification<T> : ISpecification<T>
	{
		public IQuery Query { get; protected set; }

		public Type ResultType => ((Query.Query)Query).Parent == null ? ((Query.Query)Query).EntityType : ((Query.Query)Query).Parent.Parameters.OutType;

		protected IQuery<T> Source()
		{
			return new Query<T>();
		}

		protected IQuery<TSource> Source<TSource>()
		{
			return new Query<TSource>();
		}
	}
}