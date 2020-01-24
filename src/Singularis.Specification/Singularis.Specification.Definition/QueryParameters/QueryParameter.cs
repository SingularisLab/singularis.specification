using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class QueryParameter
	{
		public Type InType { get; }
		public Type OutType { get; }
		public Expression Expression { get; }
		public QueryType QueryType { get; }

		public QueryParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType)
		{
			InType = inType;
			OutType = outType;
			Expression = expression;
			QueryType = queryType;
		}

		public virtual QueryParameter Clone()
		{
			return new QueryParameter(InType, OutType, Expression, QueryType);
		}
	}
}