using System;
using System.Linq.Expressions;
using Singularis.Specification.Domain.QueryParameters;

namespace Singularis.Specification.Domain.Query
{
    internal class OrderedQuery<T> : Query<T>, IOrderedQuery<T>
    {
        public OrderedQuery(Query parent)
            :base(parent)
        {
        }

        public IOrderedQuery<T> ThenBy<TKey>(Expression<Func<T, TKey>> selector)
        {
            Parameters = new ThenByQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(T),
                KeyType = typeof(TKey),
                Type = EQueryType.ThenBy,
                Expression = selector
            };

            return new OrderedQuery<T>(this);
        }

        public IOrderedQuery<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> selector)
        {
            Parameters = new ThenByDescendingQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(T),
                KeyType = typeof(TKey),
                Type = EQueryType.ThenByDescending,
                Expression = selector
            };

            return new OrderedQuery<T>(this);
        }
    }
}