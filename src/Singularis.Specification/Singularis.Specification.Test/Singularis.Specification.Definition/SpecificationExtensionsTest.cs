using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;
using Xunit;

namespace Singularis.Specification.Test.Singularis.Specification.Definition
{
	public class SpecificationExtensionsTest
	{
		[Fact]
		public void CanCombineEmptySpecificationWithOther()
		{
			Expression<Func<User, bool>> condition = x => x.Person != null;

			var a = new Query<User>();
			var b = new Query<User>().Where(condition);

			var result = SpecificationExtensions.Combine<User>(
				new CustomSpecification<User>(a),
				new CustomSpecification<User>(b));

			var query = (Query)result.Query;
			Assert.Equal(QueryType.Where, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(condition, query.Parameters.Expression);

			query = query.Parent;

			Assert.Equal(QueryType.Empty, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Null(query.Parameters.Expression);

			Assert.Null(query.Parent);
		}

		[Fact]
		public void CanCombineNonEmptySpecificationWithOther()
		{
			Expression<Func<User, bool>> condition1 = x => x.Person != null;
			Expression<Func<User, bool>> condition2 = x => x.Orders.Count > 1;

			var a = new Query<User>().Where(condition2);
			var b = new Query<User>().Where(condition1);

			var result = SpecificationExtensions.Combine<User>(
				new CustomSpecification<User>(a),
				new CustomSpecification<User>(b));

			var query = (Query)result.Query;
			Assert.Equal(QueryType.Where, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(condition1, query.Parameters.Expression);

			query = query.Parent;
			Assert.Equal(QueryType.Where, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(condition2, query.Parameters.Expression);

			query = query.Parent;
			Assert.Equal(QueryType.Empty, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Null(query.Parameters.Expression);

			Assert.Null(query.Parent);
		}

		[Fact]
		public void CanCombineMultipleSpecification()
		{
			Expression<Func<User, bool>> condition1 = x => x.Person != null;
			Expression<Func<User, bool>> condition2 = x => x.Orders.Count > 1;
			Expression<Func<User, bool>> condition3 = x => x.Person.Photo != null;

			var a = new Query<User>().Where(condition2);
			var b = new Query<User>().Where(condition1);
			var c = new Query<User>().Where(condition3);

			var result = SpecificationExtensions.Combine<User>(
				new CustomSpecification<User>(a),
				new CustomSpecification<User>(b),
				new CustomSpecification<User>(c));

			var query = (Query)result.Query;
			Assert.Equal(QueryType.Where, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(condition3, query.Parameters.Expression);

			query = query.Parent;
			Assert.Equal(QueryType.Where, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(condition1, query.Parameters.Expression);

			query = query.Parent;
			Assert.Equal(QueryType.Where, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(condition2, query.Parameters.Expression);

			query = query.Parent;
			Assert.Equal(QueryType.Empty, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Null(query.Parameters.Expression);

			Assert.Null(query.Parent);
		}

		[Fact]
		public void CanPerformOrForSpecification()
		{
			Expression<Func<User, bool>> condition1 = x => x.Person != null;
			Expression<Func<User, bool>> condition2 = x => x.Orders.Count > 1;

			var a = new Query<User>().Where(condition1);
			var b = new Query<User>().Where(condition2);

			var result = new CustomSpecification<User>(a).Or(new CustomSpecification<User>(b));

			var query = (Query)result.Query;
			Assert.Equal(QueryType.Where, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);

			var orExpression = ((BinaryExpression)((LambdaExpression)query.Parameters.Expression).Body);
			Assert.Equal(ExpressionType.OrElse, orExpression.NodeType);
			//Assert.AreEqual(condition1, orExpression.Left);
			//Assert.AreEqual(condition2, orExpression.Right);

			query = query.Parent;
			Assert.Equal(QueryType.Empty, query.Parameters.QueryType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Null(query.Parameters.Expression);

			Assert.Null(query.Parent);
		}
	}
}