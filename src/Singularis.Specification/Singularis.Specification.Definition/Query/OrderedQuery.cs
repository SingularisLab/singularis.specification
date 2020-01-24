using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.QueryParameters;

namespace Singularis.Specification.Definition.Query
{
	internal class OrderedQuery<T> : Query<T>, IOrderedQuery<T>
	{
		public OrderedQuery(Query parent)
			: base(parent)
		{
		}

		public IOrderedQuery<T> ThenBy<TKey>(Expression<Func<T, TKey>> selector)
		{
			var query = new OrderedQuery<T>(this)
			{
				Parameters = new ThenByQueryParameter(
					typeof(T),
					typeof(T),
					selector,
					QueryType.ThenBy,
					typeof(TKey))
			};

			return query;
		}

		public IOrderedQuery<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> selector)
		{
			var query = new OrderedQuery<T>(this)
			{
				Parameters = new ThenByDescendingQueryParameter(
					typeof(T),
					typeof(T),
					selector,
					QueryType.ThenByDescending,
					typeof(TKey))
			};

			return query;
		}
	}
}