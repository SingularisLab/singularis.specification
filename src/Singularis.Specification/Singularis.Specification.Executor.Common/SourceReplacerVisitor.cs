using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Executor.Common
{
	internal class SourceReplacerVisitor : ExpressionVisitor
	{
		class ContextCapture
		{
			public IQueryable Source { get; set; }
		}

		private readonly Func<IQuery, IQueryable> _queryEvaluator;

		public SourceReplacerVisitor(Func<IQuery, IQueryable> queryEvaluator)
		{
			_queryEvaluator = queryEvaluator;
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Arguments.Count == 1 &&
				node.Object != null &&
				node.Object.Type == typeof(IQueryContext))
			{
				var query = (IQuery)ExtractFromExpression(node.Arguments[0]);
				var source = _queryEvaluator(query);
				var contextCapture = new ContextCapture {Source = source};
				var sourceExpression = Expression.Property(Expression.Constant(contextCapture), nameof(ContextCapture.Source));

				return Expression.Convert(sourceExpression, node.Type);
			}

			return base.VisitMethodCall(node);
		}

		private static object ExtractFromExpression(Expression expression)
		{
			if (expression is MemberExpression memberExpression)
				return ExtractFromMemberExpression(memberExpression);

			if (expression is ConstantExpression constantExpression)
				return ExtractFromConstantExpression(constantExpression);

			if (expression is NewExpression newExpression)
				return ExtractFromExpression(newExpression);

			if (expression is UnaryExpression unaryExpression)
				return ExtractFromUnaryExpression(unaryExpression);

			throw new ArgumentException($"Expression has incorrect type: {expression.Type.Name}", nameof(expression));
		}

		private static object ExtractFromUnaryExpression(UnaryExpression expression)
		{
			return ExtractFromExpression(expression.Operand);
		}

		private static object ExtractFromConstantExpression(ConstantExpression expression)
		{
			if (expression.Value is Expression childExpression)
				return ExtractFromExpression(childExpression);

			return expression.Value;
		}

		private static object ExtractFromMemberExpression(MemberExpression expression)
		{
			var declaringType = expression.Member.DeclaringType;
			object declaringObject;

			if (expression.Expression is ConstantExpression constantExpression)
			{
				declaringType = constantExpression.Type;
				declaringObject = constantExpression.Value;
			}
			else
				declaringObject = expression.Member.DeclaringType;

			var member = declaringType
				.GetMember(
					expression.Member.Name,
					MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
				.Single();

			if (member.MemberType == MemberTypes.Field)
				return ((FieldInfo)member).GetValue(declaringObject);

			return ((PropertyInfo)member)
				.GetGetMethod(true)
				.Invoke(declaringObject, null);
		}
	}
}