using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition.QueryParameters;

namespace Singularis.Specification.Definition.Query
{
	public static class QueryExtensions
	{
		public static IFetchedQuery<TEntity, TProperty> Fetch<TEntity, TProperty>(
			this IQuery<TEntity> query,
			Expression<Func<TEntity, TProperty>> selector)
		{
			var result = new FetchedQuery<TEntity, TProperty>((Query)query)
			{
				Parameters = new FetchParameter(
					typeof(TEntity),
					typeof(TEntity),
					selector,
					QueryType.Fetch,
					typeof(TProperty))
			};

			return result;
		}
	}
}