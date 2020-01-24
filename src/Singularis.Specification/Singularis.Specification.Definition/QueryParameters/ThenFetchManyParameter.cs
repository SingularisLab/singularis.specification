using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class ThenFetchManyParameter : QueryParameter
	{
		public Type FetchCollectionType { get; }
		public Type FetchItemType { get; }
		public Type ParentType { get; }
		public Type RootType { get; }

		public ThenFetchManyParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType,
			Type fetchCollectionType,
			Type fetchItemType,
			Type parentType,
			Type rootType)
			: base(inType, outType, expression, queryType)
		{
			FetchCollectionType = fetchCollectionType;
			FetchItemType = fetchItemType;
			ParentType = parentType;
			RootType = rootType;
		}

		public override QueryParameter Clone()
		{
			return new ThenFetchManyParameter(InType, OutType, Expression, QueryType, FetchCollectionType, FetchItemType, ParentType, RootType);
		}
	}
}