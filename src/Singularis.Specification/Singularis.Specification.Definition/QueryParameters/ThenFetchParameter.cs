using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class ThenFetchParameter : QueryParameter
	{
		public Type FetchType { get; }
		public Type ParentType { get; }
		public Type RootType { get; }

		public bool ParentIsCollection { get; }

		public ThenFetchParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType,
			Type fetchType,
			Type parentType,
			Type rootType,
			bool parentIsCollection)
			: base(inType, outType, expression, queryType)
		{
			FetchType = fetchType;
			ParentType = parentType;
			RootType = rootType;
			ParentIsCollection = parentIsCollection;
		}

		public override QueryParameter Clone()
		{
			return new ThenFetchParameter(InType, OutType, Expression, QueryType, FetchType, ParentType, RootType, ParentIsCollection);
		}
	}
}