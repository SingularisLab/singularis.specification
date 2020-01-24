using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class ThenByQueryParameter : OrderQueryParameter
	{
		public ThenByQueryParameter(
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
			return new ThenByQueryParameter(InType, OutType, Expression, QueryType, KeyType);
		}
	}
}