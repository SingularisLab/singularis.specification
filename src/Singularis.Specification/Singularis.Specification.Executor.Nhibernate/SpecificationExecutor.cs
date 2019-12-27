using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate;
using Singularis.Specification.Domain;
using Singularis.Specification.Domain.Query;
using Singularis.Specification.Domain.QueryParameters;

namespace Singularis.Specification.Executor.Nhibernate
{
    public class SpecificationExecutor
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

		static SpecificationExecutor()
        {
            QueryMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Public | BindingFlags.Instance,
                typeof(ISession),
                nameof(ISession.Query));
				
            WhereMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Static | BindingFlags.Public,
                typeof(Queryable),
                nameof(Queryable.Where),
                new ArgumentConstraint(typeof(IQueryable<>), 1),
                new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

            SelectMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Static | BindingFlags.Public,
                typeof(Queryable),
                nameof(Queryable.Select),
                new ArgumentConstraint(typeof(IQueryable<>), 1),
                new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

            OrderByMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Static | BindingFlags.Public,
                typeof(Queryable),
                nameof(Queryable.OrderBy),
                new ArgumentConstraint(typeof(IQueryable<>), 1),
                new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

            OrderByDescendingMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Static | BindingFlags.Public,
                typeof(Queryable),
                nameof(Queryable.OrderByDescending),
                new ArgumentConstraint(typeof(IQueryable<>), 1),
                new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

            ThenByMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Static | BindingFlags.Public,
                typeof(Queryable),
                nameof(Queryable.ThenBy),
                new ArgumentConstraint(typeof(IOrderedQueryable<>), 1),
                new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

            ThenByDescendingMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Static | BindingFlags.Public,
                typeof(Queryable),
                nameof(Queryable.ThenByDescending),
                new ArgumentConstraint(typeof(IOrderedQueryable<>), 1),
                new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

            SkipMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Static | BindingFlags.Public,
                typeof(Queryable),
                nameof(Queryable.Skip),
                new ArgumentConstraint(typeof(IQueryable<>), 1),
                new ArgumentConstraint(typeof(int), 1));

            SkipWhileMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Static | BindingFlags.Public,
                typeof(Queryable),
                nameof(Queryable.SkipWhile),
                new ArgumentConstraint(typeof(IQueryable<>), 1),
                new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

            TakeMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Static | BindingFlags.Public,
                typeof(Queryable),
                nameof(Queryable.Take),
                new ArgumentConstraint(typeof(IQueryable<>), 1),
                new ArgumentConstraint(typeof(int), 1));

            TakeWhileMethodInfo = ReflectionHelper.FindMethod(
                BindingFlags.Static | BindingFlags.Public,
                typeof(Queryable),
                nameof(Queryable.TakeWhile),
                new ArgumentConstraint(typeof(IQueryable<>), 1),
                new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			GroupByMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.GroupBy),
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));
		}
		
        public IQueryable<T> ExecuteSpecification<T>(ISession session, ISpecification specification)
        {
            var queryable = CreateQueryable(session, (Query)specification.Query, (QueryContext)specification.Context);
            return ((IQueryable<T>)queryable);
        }

        public IQueryable ExecuteSpecification(ISession session, ISpecification specification)
        {
            var queryable = CreateQueryable(session, (Query)specification.Query, (QueryContext)specification.Context);
            return queryable;
        }

        internal IQueryable CreateQueryable(ISession session, Query query, QueryContext queryContext)
        {
            var queryType = GetQueryType(query);
            var chain = GetQueryChain(query);

            var result = QueryMethodInfo
                .MakeGenericMethod(queryType)
                .Invoke(session, null) as IQueryable;

            foreach (var part in chain)
            {
                switch (part.Type)
                {
                    case EQueryType.Where:
                        result = ApplyWhereExpression(session, result, part, queryContext);
                        break;

                    case EQueryType.Projection:
                        result = ApplyProjectionExpression(result, part);
                        break;

                    case EQueryType.OrderBy:
                        result = ApplyOrderByExpression(result, part);
                        break;

                    case EQueryType.OrderByDescending:
                        result = ApplyOrderByDescendingExpression(result, part);
                        break;

                    case EQueryType.ThenBy:
                        result = ApplyThenByExpression(result, part);
                        break;

                    case EQueryType.ThenByDescending:
                        result = ApplyThenByDescendingExpression(result, part);
                        break;

                    case EQueryType.Skip:
                        result = ApplySkipExpression(result, part);
                        break;

                    case EQueryType.SkipWhile:
                        result = ApplySkipWhileExpression(result, part);
                        break;

                    case EQueryType.Take:
                        result = ApplyTakeExpression(result, part);
                        break;

                    case EQueryType.TakeWhile:
                        result = ApplyTakeWhileExpression(result, part);
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

        private IQueryable ApplyWhereExpression(ISession session, IQueryable source, QueryParameter queryExpression, QueryContext queryContext)
        {
            var parameters = (WhereQueryParameter)queryExpression;
            var where = WhereMethodInfo.MakeGenericMethod(parameters.InType);

            var expression = (LambdaExpression)parameters.Expression;
            if (expression.Parameters.Any(x => x.Type == typeof(IQueryContext)))
            {
                var entityArgument = expression.Parameters.First(x => x.Type == parameters.InType);
                var visiter = new SourceReplacerVisitor(this, queryContext, session);
                var updatedExpression = (LambdaExpression)visiter.Visit(expression);
				
                expression = Expression.Lambda(updatedExpression.Body, entityArgument);
            }

            return (IQueryable)where.Invoke(source, new object[] { source, expression });
        }

        private IQueryable ApplyProjectionExpression(IQueryable source, QueryParameter queryExpression)
        {
            var parameters = (SelectQueryParameter)queryExpression;
            var select = SelectMethodInfo.MakeGenericMethod(parameters.InType, parameters.OutType);
            return (IQueryable)select.Invoke(source, new object[] { source, parameters.Expression });
        }

        private IQueryable ApplyOrderByExpression(IQueryable source, QueryParameter queryExpression)
        {
            var parameters = (OrderByQueryParameter)queryExpression;
            var orderBy = OrderByMethodInfo.MakeGenericMethod(parameters.InType, parameters.KeyType);
            return (IQueryable)orderBy.Invoke(source, new object[] { source, parameters.Expression });
        }

        private IQueryable ApplyOrderByDescendingExpression(IQueryable source, QueryParameter queryExpression)
        {
            var parameters = (OrderByDescendingQueryParameter)queryExpression;
            var orderByDescending = OrderByDescendingMethodInfo.MakeGenericMethod(parameters.InType, parameters.KeyType);
            return (IQueryable)orderByDescending.Invoke(source, new object[] { source, parameters.Expression });
        }

        private IQueryable ApplyThenByExpression(IQueryable source, QueryParameter queryExpression)
        {
            var parameters = (ThenByQueryParameter)queryExpression;
            var thenBy = ThenByMethodInfo.MakeGenericMethod(parameters.InType, parameters.KeyType);
            return (IQueryable)thenBy.Invoke(source, new object[] { source, parameters.Expression });
        }

        private IQueryable ApplyThenByDescendingExpression(IQueryable source, QueryParameter queryExpression)
        {
            var parameters = (ThenByDescendingQueryParameter)queryExpression;
            var thenByDescending = ThenByDescendingMethodInfo.MakeGenericMethod(parameters.InType, parameters.KeyType);
            return (IQueryable)thenByDescending.Invoke(source, new object[] { source, parameters.Expression });
        }

        private IQueryable ApplySkipExpression(IQueryable source, QueryParameter queryExpression)
        {
            var parameters = (SkipQueryParameter)queryExpression;
            var skip = SkipMethodInfo.MakeGenericMethod(parameters.InType);
            return (IQueryable)skip.Invoke(source, new object[] { source, parameters.Count });
        }

        private IQueryable ApplySkipWhileExpression(IQueryable source, QueryParameter queryExpression)
        {
            var parameters = (SkipWhileQueryParameter)queryExpression;
            var skipWhile = SkipWhileMethodInfo.MakeGenericMethod(parameters.InType);
            return (IQueryable)skipWhile.Invoke(source, new object[] { source, parameters.Expression });
        }

        private IQueryable ApplyTakeExpression(IQueryable source, QueryParameter queryExpression)
        {
            var parameters = (TakeQueryParameter)queryExpression;
            var take = TakeMethodInfo.MakeGenericMethod(parameters.InType);
            return (IQueryable)take.Invoke(source, new object[] { source, parameters.Count });
        }

        private IQueryable ApplyTakeWhileExpression(IQueryable source, QueryParameter queryExpression)
        {
            var parameters = (TakeWhileQueryParameter)queryExpression;
            var takeWhile = TakeWhileMethodInfo.MakeGenericMethod(parameters.InType);
            return (IQueryable)takeWhile.Invoke(source, new object[] { source, parameters.Expression });
        }
	}
}