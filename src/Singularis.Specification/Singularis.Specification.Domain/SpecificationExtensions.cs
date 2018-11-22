using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Singularis.Specification.Domain.Expressions;
using Singularis.Specification.Domain.Query;
using Singularis.Specification.Domain.QueryParameters;

namespace Singularis.Specification.Domain
{
	public static class SpecificationExtensions
	{
		public static ISpecification Combine(this ISpecification specification, ISpecification other)
		{
			var leftQuery = (Query.Query)specification.Query;
			var rightQuery = (Query.Query)other.Query;
			var leftContext = (QueryContext)specification.Context;
			var rightContext = (QueryContext)other.Context;

			var leftResult = leftQuery.Parent;

			var rightQueryItems = GetQueryChain(rightQuery);
			var rightInitial = rightQueryItems[0];

			if (leftResult.Parameters.OutType != rightInitial.Parameters.InType)
				throw new InvalidOperationException($"Left specification provide type {leftResult.Parameters.OutType} that did not match right specification {rightInitial.Parameters.InType}");

			var result = new Specification();
			leftContext.CopyTo(result.Context);
			rightContext.CopyTo(result.Context);

			var parent = leftQuery.Parent;
			
			foreach (var itm in rightQueryItems)
			{
				parent = new Query.Query(parent, itm.Parameters, itm.EntityType);
			}

			return new CustomSpecification(new Query.Query(parent, null, null), result.Context);
		}

		public static ISpecification And(this ISpecification specification, ISpecification other)
		{
			var leftQuery = (Query.Query)specification.Query;
			var rightQuery = (Query.Query)other.Query;
			var leftContext = (QueryContext)specification.Context;
			var rightContext = (QueryContext)other.Context;

			var parent = leftQuery.Parent;

			var leftQueryItems = GetQueryChain(leftQuery);
			var rightQueryItems = GetQueryChain(rightQuery);
			leftQuery = leftQueryItems.First();
			rightQuery = rightQueryItems.First();

			if (leftQueryItems.Last().Parameters.Type != EQueryType.Where)
				throw new InvalidOperationException("Left specification must ending with Where expression");

			if (rightQueryItems.Length > 1)
				throw new InvalidOperationException("Right specification contains more then 1 predicate");

			if (leftQuery.Parameters.OutType != rightQuery.Parameters.InType)
				throw new InvalidOperationException($"Left specification provide type {parent.Parameters.OutType} that did not match right specification {rightQuery.Parameters.InType}");

			var result = new Specification();
			leftContext.CopyTo(result.Context);
			rightContext.CopyTo(result.Context);

			var argumentType = parent.Parameters.OutType;

			var argumentParameter = Expression.Parameter(parent.Parameters.OutType, "obj");
			var contextParameter = Expression.Parameter(typeof(IQueryContext), "context");

			var leftExpression = (LambdaExpression)leftQuery.Parameters.Expression;
			var leftPartExpression = new ReplaceExpressionVisitor<ParameterExpression, ParameterExpression>(
					new ReplaceItem<ParameterExpression, ParameterExpression>(leftExpression.Parameters.First(x => x.Type == argumentType), argumentParameter),
					new ReplaceItem<ParameterExpression, ParameterExpression>(leftExpression.Parameters.FirstOrDefault(x => x.Type == typeof(IQueryContext)), contextParameter))
				.Visit(leftExpression.Body);

			var rightExpression = (LambdaExpression)rightQuery.Parameters.Expression;
			var rightPartExpression = new ReplaceExpressionVisitor<ParameterExpression, ParameterExpression>(
					new ReplaceItem<ParameterExpression, ParameterExpression>(rightExpression.Parameters.First(x => x.Type == argumentType), argumentParameter),
					new ReplaceItem<ParameterExpression, ParameterExpression>(rightExpression.Parameters.FirstOrDefault(x => x.Type == typeof(IQueryContext)), contextParameter))
				.Visit(rightExpression.Body);

			var resultExpressionBody = Expression.AndAlso(leftPartExpression, rightPartExpression);

			var query = new Query.Query(new Query.Query(parent.Parent, new WhereQueryParameter
			{
				Expression = Expression.Lambda(resultExpressionBody, argumentParameter, contextParameter),
				Type = EQueryType.Where,
				InType = argumentType,
				OutType = parent.Parameters.OutType
			}, parent.EntityType), null, null);

			return new CustomSpecification(query, result.Context);
		}

		public static ISpecification Or(this ISpecification specification, ISpecification other)
		{
			var leftQuery   = (Query.Query)specification.Query;
			var rightQuery  = (Query.Query)other.Query;
			var leftContext = (QueryContext)specification.Context;
			var rightContext = (QueryContext)other.Context;

			var parent = leftQuery.Parent;

			var leftQueryItems = GetQueryChain(leftQuery);
			var rightQueryItems = GetQueryChain(rightQuery);
			leftQuery = leftQueryItems.First();
			rightQuery = rightQueryItems.First();

			if (leftQueryItems.Last().Parameters.Type != EQueryType.Where)
				throw new InvalidOperationException("Left specification must ending with Where expression");

			if (rightQueryItems.Length > 1)
				throw new InvalidOperationException("Right specification contains more then 1 predicate");

			if (leftQuery.Parameters.OutType != rightQuery.Parameters.InType)
				throw new InvalidOperationException($"Left specification provide type {parent.Parameters.OutType} that did not match right specification {rightQuery.Parameters.InType}");

			var result = new Specification();
			leftContext.CopyTo(result.Context);
			rightContext.CopyTo(result.Context);

			var argumentType = parent.Parameters.OutType;

			var argumentParameter = Expression.Parameter(parent.Parameters.OutType, "obj");
			var contextParameter = Expression.Parameter(typeof(IQueryContext), "context");

			var leftExpression = (LambdaExpression)leftQuery.Parameters.Expression;
			var leftPartExpression = new ReplaceExpressionVisitor<ParameterExpression, ParameterExpression>(
					new ReplaceItem<ParameterExpression, ParameterExpression>(leftExpression.Parameters.First(x => x.Type == argumentType), argumentParameter),
					new ReplaceItem<ParameterExpression, ParameterExpression>(leftExpression.Parameters.FirstOrDefault(x => x.Type == typeof(IQueryContext)), contextParameter))
				.Visit(leftExpression.Body);

			var rightExpression = (LambdaExpression)rightQuery.Parameters.Expression;
			var rightPartExpression = new ReplaceExpressionVisitor<ParameterExpression, ParameterExpression>(
					new ReplaceItem<ParameterExpression, ParameterExpression>(rightExpression.Parameters.First(x => x.Type == argumentType), argumentParameter),
					new ReplaceItem<ParameterExpression, ParameterExpression>(rightExpression.Parameters.FirstOrDefault(x => x.Type == typeof(IQueryContext)), contextParameter))
				.Visit(rightExpression.Body);

			var resultExpressionBody = Expression.OrElse(leftPartExpression, rightPartExpression);

			var query = new Query.Query(new Query.Query(parent.Parent, new WhereQueryParameter
			{
				Expression = Expression.Lambda(resultExpressionBody, argumentParameter, contextParameter),
				Type = EQueryType.Where,
				InType = argumentType,
				OutType = parent.Parameters.OutType
			}, parent.EntityType), null, null);

			return new CustomSpecification(query, result.Context);
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
			return chain.TakeWhile(x => x.Parameters != null).ToArray();
		}
	}
}