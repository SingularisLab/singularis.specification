using System.Linq;
using System.Linq.Expressions;

namespace Singularis.Specification.Definition.Expressions
{
    class ReplaceParameterWithConstantVisitor : ExpressionVisitor
    {
        private readonly ReplaceItem<ParameterExpression, ConstantExpression>[] _replaceItems;

        public ReplaceParameterWithConstantVisitor(params ReplaceItem<ParameterExpression, ConstantExpression>[] replaceItems)
        {
            _replaceItems = replaceItems;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var parameters = node.Parameters.Where(p => _replaceItems.All(e => e.OldValue != p));
            return Expression.Lambda(Visit(node.Body), parameters);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            var replaceInfo = _replaceItems.FirstOrDefault(x => x.OldValue == node);
            if(replaceInfo == null)
                return base.VisitParameter(node);

            return replaceInfo.NewValue;
        }
    }
}