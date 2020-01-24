using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class GroupByQueryParameter : QueryParameter
	{
		public Type KeyType { get; }

		public GroupByQueryParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType,
			Type keyType)
			: base(inType, outType, expression, queryType)
		{
			KeyType = keyType;
		}

		public override QueryParameter Clone()
		{
			return new GroupByQueryParameter(InType, OutType, Expression, QueryType, KeyType);
		}
	}
}