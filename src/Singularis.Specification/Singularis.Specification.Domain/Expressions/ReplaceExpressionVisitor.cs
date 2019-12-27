using System.Linq;
using System.Linq.Expressions;

namespace Singularis.Specification.Domain.Expressions
{
    class ReplaceExpressionVisitor<TOld, TNew> : ExpressionVisitor
        where TOld: Expression
        where TNew: Expression
    {
        private readonly ReplaceItem<TOld, TNew>[] _replaceItems;

        public ReplaceExpressionVisitor(params ReplaceItem<TOld, TNew>[] replaceItems)
        {
            _replaceItems = replaceItems;
        }

        public override Expression Visit(Expression node)
        {
            var replace = _replaceItems.FirstOrDefault(x => x.OldValue == node);
            if (replace != null)
                return replace.NewValue;

            return base.Visit(node);
        }
    }
}