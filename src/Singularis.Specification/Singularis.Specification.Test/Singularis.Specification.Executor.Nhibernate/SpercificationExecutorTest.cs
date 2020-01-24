using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Moq;
using NHibernate;
using NHibernate.Linq;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Executor.Common;
using Singularis.Specification.Executor.Nhibernate;
using Singularis.Specification.Test.Shared;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;
using Singularis.Specification.Test.Singularis.Specification.Executor.Nhibernate.Spcifications;
using Xunit;
using ReflectionHelper = Singularis.Specification.Executor.Common.ReflectionHelper;

namespace Singularis.Specification.Test.Singularis.Specification.Executor.Nhibernate
{
	public class SpercificationExecutorTest
	{
		[Fact]
		public void CanTranslateWhereClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithWhere();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var whereMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.Where),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				whereMethodInfo.MakeGenericMethod(typeof(User)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}


		[Fact]
		public void CanTranslateProjectionClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithProjection();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var selectMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.Select),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				selectMethodInfo.MakeGenericMethod(typeof(User), typeof(int)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateOrderByClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithOrderBy();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var orderByMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.OrderBy),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				orderByMethodInfo.MakeGenericMethod(typeof(User), typeof(int)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateOrderByDescendingClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithOrderByDescending();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var orderByDescendingMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.OrderByDescending),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				orderByDescendingMethodInfo.MakeGenericMethod(typeof(User), typeof(int)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateThenByClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithThenBy();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var thenByMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.ThenBy),
				null,
				new ArgumentConstraint(typeof(IOrderedQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				thenByMethodInfo.MakeGenericMethod(typeof(User), typeof(int)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateThenByDescendingClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithThenByDescending();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var thenByDescendingMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.ThenByDescending),
				null,
				new ArgumentConstraint(typeof(IOrderedQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				thenByDescendingMethodInfo.MakeGenericMethod(typeof(User), typeof(int)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateSkipClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithSkip();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var skipMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.Skip),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(int), 1));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				skipMethodInfo.MakeGenericMethod(typeof(User)),
				lambda.Method);

			Assert.Equal(
				5,
				((ConstantExpression)lambda.Arguments.ElementAt(1)).Value);
		}

		[Fact]
		public void CanTranslateSkipWhileClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithSkipWhile();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var skipWhileMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.SkipWhile),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				skipWhileMethodInfo.MakeGenericMethod(typeof(User)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateTakeClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithTake();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var takeMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.Take),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(int), 1));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				takeMethodInfo.MakeGenericMethod(typeof(User)),
				lambda.Method);

			Assert.Equal(
				5,
				((ConstantExpression)lambda.Arguments.ElementAt(1)).Value);
		}

		[Fact]
		public void CanTranslateTakeWhileClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithTakeWhile();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var takeWhileMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.TakeWhile),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				takeWhileMethodInfo.MakeGenericMethod(typeof(User)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateGroupByClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithGroupBy();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var groupByMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.GroupBy),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				groupByMethodInfo.MakeGenericMethod(typeof(User), typeof(Person)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateFetchClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithFetch();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var fetchMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EagerFetchingExtensionMethods),
				nameof(EagerFetchingExtensionMethods.Fetch),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				fetchMethodInfo.MakeGenericMethod(typeof(User), typeof(Person)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateFetchCollectionClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithFetchCollection();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var fetchManyMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EagerFetchingExtensionMethods),
				nameof(EagerFetchingExtensionMethods.FetchMany),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				fetchManyMethodInfo.MakeGenericMethod(typeof(User), typeof(Order)),
				lambda.Method);

			Assert.Same(
				specification.Expression.Body,
				((UnaryExpression)((LambdaExpression)((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand).Body).Operand);
		}

		[Fact]
		public void CanTranslateThenFetchClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithThenFetch();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var thenFetchMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EagerFetchingExtensionMethods),
				nameof(EagerFetchingExtensionMethods.ThenFetch),
				null,
				new ArgumentConstraint(typeof(INhFetchRequest<,>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				thenFetchMethodInfo.MakeGenericMethod(typeof(User), typeof(Order), typeof(Address)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateThenFetchCollectionClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithThenFetchCollection();
			var queryable = executor.ExecuteSpecification(session.Object, specification);

			var thenFetchManyMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EagerFetchingExtensionMethods),
				nameof(EagerFetchingExtensionMethods.ThenFetchMany),
				null,
				new ArgumentConstraint(typeof(INhFetchRequest<,>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				thenFetchManyMethodInfo.MakeGenericMethod(typeof(User), typeof(Order), typeof(Good)),
				lambda.Method);

			Assert.Same(
				specification.Expression.Body,
				((UnaryExpression)((LambdaExpression)((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand).Body).Operand);
		}

		[Fact]
		public void CanTranslateJoinClause()
		{
			var session = new Mock<ISession>();
			var executor = new SpecificationExecutor();
			var specification = new WithJoin();
			var queryable = executor.ExecuteSpecification(session.Object, specification);
			var subQuery = executor.CreateQueryable(session.Object, (Query)specification.SubQuery);

			var joinMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.Join),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(IEnumerable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				joinMethodInfo.MakeGenericMethod(
					typeof(User),
					typeof(User),
					typeof(int),
					typeof(WithJoin.Result)),
				lambda.Method);

			//Assert.AreEqual(
			//	subQuery.Expression, 
			//	((LambdaExpression)((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand).Body);

			Assert.Same(
				specification.OuterKeySelector.Body,
				((LambdaExpression)((UnaryExpression)lambda.Arguments.ElementAt(2)).Operand).Body);

			Assert.Same(
				specification.InnerKeySelector.Body,
				((LambdaExpression)((UnaryExpression)lambda.Arguments.ElementAt(3)).Operand).Body);

			Assert.Same(
				specification.ResultSelector.Body,
				((LambdaExpression)((UnaryExpression)lambda.Arguments.ElementAt(4)).Operand).Body);
		}
	}
}
