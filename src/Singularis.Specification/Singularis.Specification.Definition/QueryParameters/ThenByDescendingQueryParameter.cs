using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class ThenByDescendingQueryParameter : OrderQueryParameter
	{
		public ThenByDescendingQueryParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType,
			Type keyType)
			: base(inType, outType, expression, queryType, keyType)
		{
		}

		public override QueryParameter Clone()
		{
			return new ThenByDescendingQueryParameter(InType, OutType, Expression, QueryType, KeyType);
		}
	}
}