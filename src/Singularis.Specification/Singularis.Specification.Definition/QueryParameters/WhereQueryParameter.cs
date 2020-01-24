using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class WhereQueryParameter : QueryParameter
	{
		public WhereQueryParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType)
			: base(inType, outType, expression, queryType)
		{
		}

		public override QueryParameter Clone()
		{
			return new WhereQueryParameter(InType, OutType, Expression, QueryType);
		}
	}
}