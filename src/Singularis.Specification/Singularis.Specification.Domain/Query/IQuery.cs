using System;
using System.Linq.Expressions;

namespace Singularis.Specification.Domain.Query
{
    public interface IQuery<T>: IQuery
    {
        IQuery<T> Where(Expression<Func<T, IQueryContext, bool>> condition);
        IQuery<T> Where(Expression<Func<T, bool>> condition);

        IQuery<TProjection> Projection<TProjection>(Expression<Func<T, TProjection>> projector);

        IOrderedQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> selector);
        IOrderedQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> selector);

        IQuery<T> Skip(int count);
        IQuery<T> SkipWhile(Expression<Func<T, bool>> condition);

        IQuery<T> Take(int count);
        IQuery<T> TakeWhile(Expression<Func<T, bool>> condition);
    }

    public interface IQuery
    {
    }
}