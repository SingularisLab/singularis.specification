using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Executor.Common;
using Singularis.Specification.Executor.EntityFramework;
using Singularis.Specification.Test.Shared;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;
using Singularis.Specification.Test.Singularis.Specification.Executor.EntityFramework.Fake;
using Singularis.Specification.Test.Singularis.Specification.Executor.EntityFramework.Spcifications;
using Xunit;

namespace Singularis.Specification.Test.Singularis.Specification.Executor.EntityFramework
{
	public class SpercificationExecutorTest
	{
		[Fact]
		public void CanTranslateWhereClause()
		{
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithWhere();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithProjection();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithOrderBy();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithOrderByDescending();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithThenBy();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithThenByDescending();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithSkip();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithSkipWhile();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithTake();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithTakeWhile();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithGroupBy();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

			var groupByMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(Queryable),
				nameof(Queryable.GroupBy),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Equal(
				groupByMethodInfo.MakeGenericMethod(typeof(User), typeof(Person)),
				lambda.Method);

			Assert.Equal(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateFetchClause()
		{
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithFetch();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

			var fetchMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EntityFrameworkQueryableExtensions),
				nameof(EntityFrameworkQueryableExtensions.Include),
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
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithFetchCollection();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

			var fetchMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EntityFrameworkQueryableExtensions),
				nameof(EntityFrameworkQueryableExtensions.Include),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				fetchMethodInfo.MakeGenericMethod(typeof(User), typeof(ICollection<Order>)),
				lambda.Method);

			Assert.Same(
				specification.Expression.Body,
				((LambdaExpression)((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand).Body);
		}

		[Fact]
		public void CanTranslateThenFetchAfterCollectionClause()
		{
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithThenFetchAfterCollection();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

			var thenFetchAfterCollectionMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(EntityFrameworkQueryableExtensions),
				nameof(EntityFrameworkQueryableExtensions.ThenInclude),
				null,
				new ArgumentConstraint(typeof(IIncludableQueryable<,>), 1),
				new ArgumentConstraint(typeof(Expression<>).MakeGenericType(typeof(Func<,>)), 2));

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				thenFetchAfterCollectionMethodInfo.MakeGenericMethod(typeof(User), typeof(Order), typeof(Address)),
				lambda.Method);

			Assert.Same(
				specification.Expression,
				((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand);
		}

		[Fact]
		public void CanTranslateThenFetchClause()
		{
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithThenFetch();
			var queryable = executor.ExecuteSpecification(context.Object, specification);

			var thenFetchMethodInfo = typeof(EntityFrameworkQueryableExtensions)
				.GetMethods(BindingFlags.Public | BindingFlags.Static)
				.Where(x => x.Name == nameof(EntityFrameworkQueryableExtensions.ThenInclude))
				.First(x =>
				{
					var arguments = x.GetParameters();
					var includableQuerySecondArguments = arguments.First().ParameterType.GetGenericArguments().ElementAt(1);
					return !includableQuerySecondArguments.IsGenericType || !(typeof(IEnumerable<>).IsAssignableFrom(includableQuerySecondArguments.GetGenericTypeDefinition()));
				});

			var lambda = (MethodCallExpression)queryable.Expression;
			Assert.Equal(ExpressionType.Call, lambda.NodeType);
			Assert.Same(
				thenFetchMethodInfo.MakeGenericMethod(typeof(User), typeof(Person), typeof(byte[])),
				lambda.Method);

			Assert.Same(
				specification.Expression.Body,
				((LambdaExpression)((UnaryExpression)lambda.Arguments.ElementAt(1)).Operand).Body);
		}

		[Fact]
		public void CanTranslateJoinClause()
		{
			var context = new Mock<Context>();
			context
				.Setup(x => x.Set<User>())
				.Returns(new FakeSet<User>());

			var executor = new SpecificationExecutor();
			var specification = new WithJoin();
			var queryable = executor.ExecuteSpecification(context.Object, specification);
			var subQuery = executor.CreateQueryable(context.Object, (Query)specification.SubQuery);

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
