using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class FetchManyParameter : QueryParameter
	{
		public Type FetchItemType { get; }
		public Type FetchCollectionType { get; }

		public FetchManyParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType,
			Type fetchItemType,
			Type fetchCollectionType)
			: base(inType, outType, expression, queryType)
		{
			FetchItemType = fetchItemType;
			FetchCollectionType = fetchCollectionType;
		}

		public override QueryParameter Clone()
		{
			return new FetchManyParameter(InType, OutType, Expression, QueryType, FetchItemType, FetchCollectionType);
		}
	}
}