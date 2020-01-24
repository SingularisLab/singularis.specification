using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class OrderByQueryParameter : OrderQueryParameter
	{
		public OrderByQueryParameter(
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
			return new OrderByQueryParameter(InType, OutType, Expression, QueryType, KeyType);
		}
	}
}