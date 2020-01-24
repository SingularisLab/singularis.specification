using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Singularis.Specification.Definition.Expressions;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Definition.QueryParameters;

namespace Singularis.Specification.Definition
{
	public static class SpecificationExtensions
	{
		private static readonly MethodInfo CombineMethodInfo;

		static SpecificationExtensions()
		{
			CombineMethodInfo = typeof(SpecificationExtensions)
				.GetMethods(BindingFlags.Public | BindingFlags.Static)
				.First(x => x.Name == nameof(Combine) && x.IsGenericMethod && x.GetParameters().Length == 2);
		}

		public static ISpecification<T> Combine<T>(params ISpecification[] specifications)
		{
			return (ISpecification<T>)Combine(specifications);
		}

		public static ISpecification Combine(params ISpecification[] specifications)
		{
			if (!specifications.Any())
				throw new ArgumentException("Collection must contains at lease two elements", nameof(specifications));

			var result = specifications.First();
			for (int i = 1; i < specifications.Length; i++)
			{
				result = Combine(result, specifications[i]);
			}

			return result;
		}

		private static ISpecification Combine(ISpecification left, ISpecification right)
		{
			var leftQuery = (Query.Query)left.Query;
			var rightQuery = (Query.Query)right.Query;

			var result = CombineMethodInfo
				.MakeGenericMethod(leftQuery.Parameters.InType, rightQuery.Parameters.OutType)
				.Invoke(null, new[] { left, right });

			return (ISpecification)result;
		}

		public static ISpecification<TRight> Combine<TLeft, TRight>(this ISpecification<TLeft> specification, ISpecification<TRight> other)
		{
			var leftQuery = (Query.Query)specification.Query;
			var rightQuery = (Query.Query)other.Query;

			var rightQueryItems = GetQueryChain(rightQuery);
			var rightFirstClause = rightQueryItems[0];

			if (leftQuery.Parameters.OutType != rightFirstClause.Parameters.InType)
				throw new InvalidOperationException($"Left specification provide type {leftQuery.Parameters.OutType} that did not match right specification {rightFirstClause.Parameters.InType}");

			var query = leftQuery;
			foreach (var itm in rightQueryItems)
			{
				if (itm.Parameters.QueryType == QueryType.Empty)
					continue;

				query = new Query.Query(query, itm.Parameters, itm.EntityType);
			}

			return new CustomSpecification<TRight>(query);
		}

		public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
		{
			var leftQuery = (Query.Query)left.Query;
			var rightQuery = (Query.Query)right.Query;

			var leftQueryItems = GetQueryChain(leftQuery);
			var rightQueryItems = GetQueryChain(rightQuery);
			leftQuery = leftQueryItems.Last();
			rightQuery = rightQueryItems.SkipWhile(x => x.Parameters.QueryType == QueryType.Empty).First();

			if (leftQueryItems.Last().Parameters.QueryType != QueryType.Where)
				throw new InvalidOperationException("Left specification must ending with Where expression");

			if (rightQueryItems.SkipWhile(x => x.Parameters.QueryType == QueryType.Empty).Count(x => x.Parameters.QueryType != QueryType.Where) > 0)
				throw new InvalidOperationException("Right specification should contains only one Where clause");

			var argumentParameter = Expression.Parameter(typeof(T), "obj");
			var contextParameter = Expression.Parameter(typeof(IQueryContext), "context");

			var leftExpression = (LambdaExpression)leftQuery.Parameters.Expression;
			var leftPartExpression = new ReplaceExpressionVisitor<ParameterExpression, ParameterExpression>(
					new ReplaceItem<ParameterExpression, ParameterExpression>(leftExpression.Parameters.First(x => x.Type == typeof(T)), argumentParameter),
					new ReplaceItem<ParameterExpression, ParameterExpression>(leftExpression.Parameters.FirstOrDefault(x => x.Type == typeof(IQueryContext)), contextParameter))
				.Visit(leftExpression.Body);

			var rightExpression = (LambdaExpression)rightQuery.Parameters.Expression;
			var rightPartExpression = new ReplaceExpressionVisitor<ParameterExpression, ParameterExpression>(
					new ReplaceItem<ParameterExpression, ParameterExpression>(rightExpression.Parameters.First(x => x.Type == typeof(T)), argumentParameter),
					new ReplaceItem<ParameterExpression, ParameterExpression>(rightExpression.Parameters.FirstOrDefault(x => x.Type == typeof(IQueryContext)), contextParameter))
				.Visit(rightExpression.Body);

			var resultExpressionBody = Expression.OrElse(leftPartExpression, rightPartExpression);

			var query = new Query.Query(
				leftQueryItems.Last().Parent,
				new WhereQueryParameter(
					typeof(T),
					typeof(T),
					Expression.Lambda(resultExpressionBody, argumentParameter, contextParameter),
					QueryType.Where),
				leftQuery.EntityType);

			return new CustomSpecification<T>(query);
		}

		static Query.Query[] GetQueryChain(this Query.Query query)
		{
			var chain = new List<Query.Query>();

			while (query != null)
			{
				chain.Add(query);
				query = query.Parent;
			}

			chain.Reverse();
			return chain.ToArray();
		}
	}
}