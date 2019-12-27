using System.Linq.Expressions;
using NHibernate;
using Singularis.Specification.Domain;
using Singularis.Specification.Domain.Query;

namespace Singularis.Specification.Executor.Nhibernate
{
    class SourceReplacerVisitor : ExpressionVisitor
    {
        private readonly SpecificationExecutor _executor;
        private readonly IQueryContext _context;
        private readonly ISession _session;
		
        public SourceReplacerVisitor(SpecificationExecutor executor, IQueryContext context, ISession session)
        {
            _executor = executor;
            _context = context;
            _session = session;
        }
		
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Arguments.Count == 1 &&
                node.Object != null &&
                node.Object.Type == typeof(IQueryContext))
            {
                var value = (string)((ConstantExpression)node.Arguments[0]).Value;
				
                var query = _context.GetQuery(value);
                var source = _executor.CreateQueryable(_session, (Query)query, (QueryContext)_context);

                var sourceExpression = Expression.Constant(source, source.GetType());

                return sourceExpression;
            }

            return base.VisitMethodCall(node);
        }
    }
}