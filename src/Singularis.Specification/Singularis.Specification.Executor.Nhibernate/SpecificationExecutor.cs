﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate;
using NHibernate.Linq;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Definition.QueryParameters;
using Singularis.Specification.Executor.Common;
using ReflectionHelper = Singularis.Specification.Executor.Common.ReflectionHelper;

namespace Singularis.Specification.Executor.Nhibernate
{
	class SpecificationExecutor
	{
		private static readonly MethodInfo QueryMethodInfo;
		private static readonly MethodInfo WhereMethodInfo;
		private static readonly MethodInfo SelectMethodInfo;
		private static readonly MethodInfo OrderByMethodInfo;
		private static readonly MethodInfo OrderByDescendingMethodInfo;
		private static readonly MethodInfo ThenByMethodInfo;
		private static readonly MethodInfo ThenByDescendingMethodInfo;
		private static readonly MethodInfo SkipMethodInfo;
		private static readonly MethodInfo SkipWhileMethodInfo;
		private static readonly MethodInfo TakeMethodInfo;
		private static readonly MethodInfo TakeWhileMethodInfo;
		private static readonly MethodInfo GroupByMethodInfo;
		private static readonly MethodInfo FetchMethodInfo;
		private static readonly MethodInfo FetchManyMethodInfo;
		private static readonly MethodInfo ThenFetchMethodInfo;
		private static readonly MethodInfo ThenFetchManyMethodInfo;
		private static readonly MethodInfo JoinMethodInfo;

		static SpecificationExecutor()
		{
			QueryMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Public | BindingFlags.Instance,
				typeof(ISession),
				nameof(ISession.Query),
				null);

			WhereMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.Where),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			SelectMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.Select),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			OrderByMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.OrderBy),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			OrderByDescendingMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.OrderByDescending),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			ThenByMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.ThenBy),
				null,
				new ArgumentConstraint(typeof(IOrderedQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			ThenByDescendingMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.ThenByDescending),
				null,
				new ArgumentConstraint(typeof(IOrderedQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			SkipMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.Skip),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(int), 1));

			SkipWhileMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.SkipWhile),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			TakeMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.Take),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(int), 1));

			TakeWhileMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.TakeWhile),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			GroupByMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.GroupBy),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			FetchMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EagerFetchingExtensionMethods),
				nameof(EagerFetchingExtensionMethods.Fetch),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			FetchManyMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EagerFetchingExtensionMethods),
				nameof(EagerFetchingExtensionMethods.FetchMany),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			ThenFetchMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EagerFetchingExtensionMethods),
				nameof(EagerFetchingExtensionMethods.ThenFetch),
				null,
				new ArgumentConstraint(typeof(INhFetchRequest<,>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			ThenFetchManyMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EagerFetchingExtensionMethods),
				nameof(EagerFetchingExtensionMethods.ThenFetchMany),
				null,
				new ArgumentConstraint(typeof(INhFetchRequest<,>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			JoinMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.Join),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(IEnumerable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,,>)), 2));
		}

		public IQueryable<T> ExecuteSpecification<T>(ISession session, ISpecification specification)
		{
			var queryable = CreateQueryable(session, (Query)specification.Query);
			return ((IQueryable<T>)queryable);
		}

		public IQueryable ExecuteSpecification(ISession session, ISpecification specification)
		{
			var queryable = CreateQueryable(session, (Query)specification.Query);
			return queryable;
		}

		internal IQueryable CreateQueryable(ISession session, Query query)
		{
			var queryType = GetQueryType(query);
			var chain = GetQueryChain(query);

			var result = QueryMethodInfo
				.MakeGenericMethod(queryType)
				.Invoke(session, null) as IQueryable;

			foreach (var part in chain)
			{
				switch (part.QueryType)
				{
					case QueryType.Where:
						result = ApplyWhereExpression(session, result, part);
						break;

					case QueryType.Projection:
						result = ApplyProjectionExpression(result, part);
						break;

					case QueryType.OrderBy:
						result = ApplyOrderByExpression(result, part);
						break;

					case QueryType.OrderByDescending:
						result = ApplyOrderByDescendingExpression(result, part);
						break;

					case QueryType.ThenBy:
						result = ApplyThenByExpression(result, part);
						break;

					case QueryType.ThenByDescending:
						result = ApplyThenByDescendingExpression(result, part);
						break;

					case QueryType.Skip:
						result = ApplySkipExpression(result, part);
						break;

					case QueryType.SkipWhile:
						result = ApplySkipWhileExpression(result, part);
						break;

					case QueryType.Take:
						result = ApplyTakeExpression(result, part);
						break;

					case QueryType.TakeWhile:
						result = ApplyTakeWhileExpression(result, part);
						break;

					case QueryType.GroupBy:
						result = ApplyGroupByExpression(result, part);
						break;

					case QueryType.Fetch:
						result = ApplyFetchExpression(result, part);
						break;

					case QueryType.ThenFetch:
						result = ApplyThenFetchExpression(result, part);
						break;

					case QueryType.Join:
						result = ApplyJoinExpression(session, result, part);
						break;
				}
			}

			return result;
		}

		private Type GetQueryType(Query query)
		{
			Type queryType = null;

			while (query != null)
			{
				if (query.Parent == null && queryType == null)
					queryType = query.EntityType;
				else if (query.Parent != null)
					queryType = query.Parent.EntityType;

				query = query.Parent;
			}

			return queryType;
		}

		private List<QueryParameter> GetQueryChain(Query query)
		{
			var chain = new List<QueryParameter>();

			while (query != null)
			{
				if (query.Parameters != null)
					chain.Add(query.Parameters);

				query = query.Parent;
			}

			chain.Reverse();
			return chain;
		}

		private IQueryable ApplyWhereExpression(ISession session, IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (WhereQueryParameter)queryExpression;
			var where = WhereMethodInfo.MakeGenericMethod(parameters.InType);

			var expression = (LambdaExpression)parameters.Expression;
			if (expression.Parameters.Any(x => x.Type == typeof(IQueryContext)))
			{
				var entityArgument = expression.Parameters.First(x => x.Type == parameters.InType);
				var visiter = new SourceReplacerVisitor(q => CreateQueryable(session, (Query)q));
				var updatedExpression = (LambdaExpression)visiter.Visit(expression);

				expression = Expression.Lambda(updatedExpression.Body, entityArgument);
			}

			return (IQueryable)where.Invoke(source, new object[] { source, expression });
		}

		private IQueryable ApplyProjectionExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (SelectQueryParameter)queryExpression;
			var action = SelectMethodInfo.MakeGenericMethod(parameters.InType, parameters.OutType);
			return (IQueryable)action.Invoke(source, new object[] { source, parameters.Expression });
		}

		private IQueryable ApplyOrderByExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (OrderByQueryParameter)queryExpression;
			var action = OrderByMethodInfo.MakeGenericMethod(parameters.InType, parameters.KeyType);
			return (IQueryable)action.Invoke(source, new object[] { source, parameters.Expression });
		}

		private IQueryable ApplyOrderByDescendingExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (OrderByDescendingQueryParameter)queryExpression;
			var action = OrderByDescendingMethodInfo.MakeGenericMethod(parameters.InType, parameters.KeyType);
			return (IQueryable)action.Invoke(source, new object[] { source, parameters.Expression });
		}

		private IQueryable ApplyThenByExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (ThenByQueryParameter)queryExpression;
			var action = ThenByMethodInfo.MakeGenericMethod(parameters.InType, parameters.KeyType);
			return (IQueryable)action.Invoke(source, new object[] { source, parameters.Expression });
		}

		private IQueryable ApplyThenByDescendingExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (ThenByDescendingQueryParameter)queryExpression;
			var action = ThenByDescendingMethodInfo.MakeGenericMethod(parameters.InType, parameters.KeyType);
			return (IQueryable)action.Invoke(source, new object[] { source, parameters.Expression });
		}

		private IQueryable ApplySkipExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (SkipQueryParameter)queryExpression;
			var action = SkipMethodInfo.MakeGenericMethod(parameters.InType);
			return (IQueryable)action.Invoke(source, new object[] { source, parameters.Count });
		}

		private IQueryable ApplySkipWhileExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (SkipWhileQueryParameter)queryExpression;
			var action = SkipWhileMethodInfo.MakeGenericMethod(parameters.InType);
			return (IQueryable)action.Invoke(source, new object[] { source, parameters.Expression });
		}

		private IQueryable ApplyTakeExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (TakeQueryParameter)queryExpression;
			var action = TakeMethodInfo.MakeGenericMethod(parameters.InType);
			return (IQueryable)action.Invoke(source, new object[] { source, parameters.Count });
		}

		private IQueryable ApplyTakeWhileExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (TakeWhileQueryParameter)queryExpression;
			var action = TakeWhileMethodInfo.MakeGenericMethod(parameters.InType);
			return (IQueryable)action.Invoke(source, new object[] { source, parameters.Expression });
		}

		private IQueryable ApplyGroupByExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (GroupByQueryParameter)queryExpression;
			var action = GroupByMethodInfo.MakeGenericMethod(parameters.InType, parameters.KeyType);
			return (IQueryable)action.Invoke(source, new object[] { source, parameters.Expression });
		}

		private IQueryable ApplyFetchExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (FetchParameter)queryExpression;
			MethodInfo action;
			Expression expression;
			if (typeof(IEnumerable).IsAssignableFrom(parameters.FetchType))
			{
				var fetchItemType = parameters.FetchType.GetGenericArguments().First();
				action = FetchManyMethodInfo.MakeGenericMethod(parameters.InType, fetchItemType);
				var selectorExpression = ((LambdaExpression)parameters.Expression);
				expression = Expression.Lambda(
					Expression.Convert(selectorExpression.Body, typeof(IEnumerable<>).MakeGenericType(fetchItemType)),
					selectorExpression.Parameters);
			}
			else
			{
				action = FetchMethodInfo.MakeGenericMethod(parameters.InType, parameters.FetchType);
				expression = parameters.Expression;
			}

			return (IQueryable)action.Invoke(source, new object[] { source, expression });
		}

		private IQueryable ApplyThenFetchExpression(IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (ThenFetchParameter)queryExpression;
			MethodInfo action;
			Expression expression;
			if (typeof(IEnumerable).IsAssignableFrom(parameters.FetchType))
			{
				var fetchItemType = parameters.FetchType.GetGenericArguments().First();
				action = ThenFetchManyMethodInfo.MakeGenericMethod(parameters.InType, parameters.ParentType, fetchItemType);
				var selectorExpression = ((LambdaExpression)parameters.Expression);
				expression = Expression.Lambda(
					Expression.Convert(selectorExpression.Body, typeof(IEnumerable<>).MakeGenericType(fetchItemType)),
					selectorExpression.Parameters);
			}
			else
			{
				action = ThenFetchMethodInfo.MakeGenericMethod(parameters.InType, parameters.ParentType, parameters.FetchType);
				expression = parameters.Expression;
			}

			return (IQueryable)action.Invoke(source, new object[] { source, expression });
		}

		private IQueryable ApplyJoinExpression(ISession session, IQueryable source, QueryParameter queryExpression)
		{
			var parameters = (JoinQueryParameter)queryExpression;
			var action = JoinMethodInfo.MakeGenericMethod(parameters.OuterType, parameters.InnerType, parameters.KeyType, parameters.OutType);
			var innerSource = CreateQueryable(session, (Query)parameters.InnerSource);

			return (IQueryable)action.Invoke(source, new object[] {
				source,
				innerSource,
				parameters.OuterKeySelectorExpression,
				parameters.InnerKeySelectorExpression,
				parameters.Expression });
		}
	}
}