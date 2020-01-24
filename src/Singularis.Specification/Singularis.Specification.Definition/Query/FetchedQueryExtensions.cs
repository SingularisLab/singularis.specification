using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Singularis.Specification.Definition.QueryParameters;

namespace Singularis.Specification.Definition.Query
{
	public static class FetchedQueryExtensions
	{
		public static IFetchedQuery<TEntity, TProperty> ThenFetch<TEntity, TParent, TProperty>(
			this IFetchedQuery<TEntity, TParent> query,
			Expression<Func<TParent, TProperty>> selector)
		{
			var result = new FetchedQuery<TEntity, TProperty>((Query)query)
			{
				Parameters = new ThenFetchParameter(
					typeof(TEntity),
					typeof(TEntity),
					selector,
					QueryType.ThenFetch,
					typeof(TProperty),
					typeof(TParent),
					typeof(TEntity),
					false)
			};

			return result;
		}

		public static IFetchedQuery<TEntity, TProperty> ThenFetch<TEntity, TParent, TProperty>(
			this IFetchedQuery<TEntity, IEnumerable<TParent>> query,
			Expression<Func<TParent, TProperty>> selector)
		{
			var result = new FetchedQuery<TEntity, TProperty>((Query)query)
			{
				Parameters = new ThenFetchParameter(
					typeof(TEntity),
					typeof(TEntity),
					selector,
					QueryType.ThenFetch,
					typeof(TProperty),
					typeof(TParent),
					typeof(TEntity),
					true)
			};

			return result;
		}
	}
}