using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition.QueryParameters
{
	internal class JoinQueryParameter : QueryParameter
	{
		public IQuery InnerSource { get; }
		public Type InnerType { get; }
		public Type OuterType { get; }
		public Type KeyType { get; }
		public Expression InnerKeySelectorExpression { get; }
		public Expression OuterKeySelectorExpression { get; }

		public JoinQueryParameter(
			Type inType,
			Type outType,
			Expression expression,
			QueryType queryType,
			IQuery innerSource,
			Type innerType,
			Type outerType,
			Type keyType,
			Expression innerKeySelectorExpression,
			Expression outerKeySelectorExpression)
			: base(inType, outType, expression, queryType)
		{
			InnerSource = innerSource;
			InnerType = innerType;
			OuterType = outerType;
			KeyType = keyType;
			InnerKeySelectorExpression = innerKeySelectorExpression;
			OuterKeySelectorExpression = outerKeySelectorExpression;
		}

		public override QueryParameter Clone()
		{
			return new JoinQueryParameter(InnerType, OuterType, Expression, QueryType, InnerSource, InnerType, OuterType, KeyType, InnerKeySelectorExpression, OuterKeySelectorExpression);
		}
	}
}