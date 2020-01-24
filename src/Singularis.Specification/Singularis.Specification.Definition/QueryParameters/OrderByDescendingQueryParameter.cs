using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class OrderByDescendingQueryParameter : OrderQueryParameter
	{
		public OrderByDescendingQueryParameter(
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
			return new OrderByDescendingQueryParameter(InType, OutType, Expression, QueryType, KeyType);
		}
	}
}