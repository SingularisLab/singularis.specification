using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class TakeWhileQueryParameter : QueryParameter
	{
		public TakeWhileQueryParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType)
			: base(inType, outType, expression, queryType)
		{
		}

		public override QueryParameter Clone()
		{
			return new TakeWhileQueryParameter(InType, OutType, Expression, QueryType);
		}
	}
}