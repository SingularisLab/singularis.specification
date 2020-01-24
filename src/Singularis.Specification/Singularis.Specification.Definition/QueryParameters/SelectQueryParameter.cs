using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class SelectQueryParameter : QueryParameter
	{
		public SelectQueryParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType)
			: base(inType, outType, expression, queryType)
		{
		}

		public override QueryParameter Clone()
		{
			return new SelectQueryParameter(InType, OutType, Expression, QueryType);
		}
	}
}