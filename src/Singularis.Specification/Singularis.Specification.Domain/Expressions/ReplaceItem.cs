using System.Linq.Expressions;

namespace Singularis.Specification.Domain.Expressions
{
    class ReplaceItem<TOld, TNew> 
        where TOld: Expression
        where TNew : Expression
    {
        public TOld OldValue { get; }
        public TNew NewValue { get; }

        public ReplaceItem(TOld oldValue, TNew newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}