using System;
using System.Linq;
using System.Linq.Expressions;
using Singularis.Specification.Definition.QueryParameters;

namespace Singularis.Specification.Definition.Query
{
	internal class Query : IQuery
	{
		public Type EntityType { get; protected set; }
		public Query Parent { get; protected set; }
		public QueryParameter Parameters { get; internal set; }

		protected Query()
		{
		}

		protected Query(Query parent)
		{
			Parent = parent;
		}

		public Query(Query parent, QueryParameter parameters, Type entityType)
		{
			Parent = parent;
			Parameters = parameters;
			EntityType = entityType;
		}
	}

	internal class Query<T> : Query, IQuery<T>
	{
		public Query()
		{
			EntityType = typeof(T);
			Parameters = new EmptyQueryParameter(
				typeof(T),
				typeof(T),
				null,
				QueryType.Empty);
		}

		public Query(Query parent)
			: base(parent)
		{
		}

		public IQuery<T> Where(Expression<Func<T, IQueryContext, bool>> condition)
		{
			var query = new Query<T>(this)
			{
				Parameters = new WhereQueryParameter(
					typeof(T),
					typeof(T),
					condition,
					QueryType.Where)
			};

			return query;
		}

		public IQuery<T> Where(Expression<Func<T, bool>> condition)
		{
			var query = new Query<T>(this)
			{
				Parameters = new WhereQueryParameter(
					typeof(T),
					typeof(T),
					condition,
					QueryType.Where)
			};

			return query;
		}

		public IQuery<TProjection> Projection<TProjection>(Expression<Func<T, TProjection>> projector)
		{
			var query = new Query<TProjection>(this)
			{
				Parameters = new SelectQueryParameter(
					typeof(T),
					typeof(TProjection),
					projector,
					QueryType.Projection)
			};

			return query;
		}

		public IOrderedQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> selector)
		{
			var query = new OrderedQuery<T>(this)
			{
				Parameters = new OrderByQueryParameter(
					typeof(T),
					typeof(T),
					selector,
					QueryType.OrderBy,
					typeof(TKey))
			};

			return query;
		}

		public IOrderedQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> selector)
		{
			var query = new OrderedQuery<T>(this)
			{
				Parameters = new OrderByDescendingQueryParameter(
					typeof(T),
					typeof(T),
					selector,
					QueryType.OrderByDescending,
					typeof(TKey))
			};

			return query;
		}

		public IQuery<T> Skip(int count)
		{
			var query = new Query<T>(this)
			{
				Parameters = new SkipQueryParameter(
					typeof(T),
					typeof(T),
					null,
					QueryType.Skip,
					count)
			};

			return query;
		}

		public IQuery<T> SkipWhile(Expression<Func<T, bool>> condition)
		{
			var query = new Query<T>(this)
			{
				Parameters = new SkipWhileQueryParameter(
					typeof(T),
					typeof(T),
					condition,
					QueryType.SkipWhile)
			};

			return query;
		}

		public IQuery<T> Take(int count)
		{
			var query = new Query<T>(this)
			{
				Parameters = new TakeQueryParameter(
					typeof(T),
					typeof(T),
					null,
					QueryType.Take,
					count)
			};

			return query;
		}

		public IQuery<T> TakeWhile(Expression<Func<T, bool>> condition)
		{
			var query = new Query<T>(this)
			{
				Parameters = new TakeWhileQueryParameter(
					typeof(T),
					typeof(T),
					condition,
					QueryType.TakeWhile)
			};

			return query;
		}

		public IQuery<IGrouping<TKey, T>> GroupBy<TKey>(Expression<Func<T, TKey>> selector)
		{
			var query = new Query<IGrouping<TKey, T>>(this)
			{
				Parameters = new GroupByQueryParameter(
					typeof(T),
					typeof(IGrouping<TKey, T>),
					selector,
					QueryType.GroupBy,
					typeof(TKey))
			};

			return query;
		}

		public IQuery<TResult> Join<TKey, TInner, TResult>(
			IQuery<TInner> innerSource,
			Expression<Func<T, TKey>> outerKeySelecotr,
			Expression<Func<TInner, TKey>> innerKeySelector,
			Expression<Func<T, TInner, TResult>> resultSelector)
		{
			var query = new Query<TResult>(this)
			{
				Parameters = new JoinQueryParameter(
					null,
					typeof(TResult),
					resultSelector,
					QueryType.Join,
					innerSource,
					typeof(TInner),
					typeof(T),
					typeof(TKey),
					innerKeySelector,
					outerKeySelecotr)
			};

			return query;
		}
	}
}