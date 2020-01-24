using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class TakeQueryParameter : QueryParameter
	{
		public int Count { get; }

		public TakeQueryParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType,
			int count)
			: base(inType, outType, expression, queryType)
		{
			Count = count;
		}

		public override QueryParameter Clone()
		{
			return new TakeQueryParameter(InType, OutType, Expression, QueryType, Count);
		}
	}
}