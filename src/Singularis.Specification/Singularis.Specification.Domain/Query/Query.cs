using System;
using System.Linq.Expressions;
using Singularis.Specification.Domain.QueryParameters;

namespace Singularis.Specification.Domain.Query
{
    internal class Query: IQuery
    {
        public Type EntityType { get; protected set; }
        public Query Parent { get; protected set; }
        public QueryParameter Parameters { get; protected set; }

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
    
    internal class Query<T>: Query, IQuery<T>
    {
        public Query()
        {
            EntityType = typeof(T);
        }
		
        public Query(Query parent)
            :base(parent)
        {
        }

        public IQuery<T> Where(Expression<Func<T, IQueryContext, bool>> condition)
        {
            Parameters = new WhereQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(T),
                Expression = condition,
                Type = EQueryType.Where
            };

            return new Query<T>(this);
        }

        public IQuery<T> Where(Expression<Func<T, bool>> condition)
        {
            Parameters = new WhereQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(T),
                Expression = condition,
                Type = EQueryType.Where
            };

            return new Query<T>(this);
        }

        public IQuery<TProjection> Projection<TProjection>(Expression<Func<T, TProjection>> projector)
        {
            Parameters = new SelectQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(TProjection),
                Expression = projector,
                Type = EQueryType.Projection
            };

            return new Query<TProjection>(this);
        }

        public IOrderedQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> selector)
        {
            Parameters = new OrderByQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(T),
                KeyType = typeof(TKey),
                Type = EQueryType.OrderBy,
                Expression = selector
            };

            return new OrderedQuery<T>(this);
        }

        public IOrderedQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> selector)
        {
            Parameters = new OrderByDescendingQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(T),
                KeyType = typeof(TKey),
                Type = EQueryType.OrderByDescending,
                Expression = selector
            };

            return new OrderedQuery<T>(this);
        }

        public IQuery<T> Skip(int count)
        {
            Parameters = new SkipQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(T),
                Type = EQueryType.Skip,
                Count = count
            };

            return new Query<T>(this);
        }

        public IQuery<T> SkipWhile(Expression<Func<T, bool>> condition)
        {
            Parameters = new SkipWhileQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(T),
                Type = EQueryType.SkipWhile,
                Expression = condition
            };

            return new Query<T>(this);
        }

        public IQuery<T> Take(int count)
        {
            Parameters = new TakeQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(T),
                Type = EQueryType.Take,
                Count = count
            };

            return new Query<T>(this);
        }

        public IQuery<T> TakeWhile(Expression<Func<T, bool>> condition)
        {
            Parameters = new TakeWhileQueryParameter
            {
                InType = typeof(T),
                OutType = typeof(T),
                Type = EQueryType.TakeWhile,
                Expression = condition
            };

            return new Query<T>(this);
        }
    }
}