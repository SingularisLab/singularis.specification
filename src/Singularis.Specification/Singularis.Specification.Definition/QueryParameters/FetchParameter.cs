using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class FetchParameter : QueryParameter
	{
		public Type FetchType { get; }

		public FetchParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType,
			Type fetchType)
			: base(inType, outType, expression, queryType)
		{
			FetchType = fetchType;
		}

		public override QueryParameter Clone()
		{
			return new FetchParameter(InType, OutType, Expression, QueryType, FetchType);
		}
	}
}