using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Definition.QueryParameters;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;
using Xunit;

namespace Singularis.Specification.Test.Singularis.Specification.Definition
{
	public class QueryTest
	{
		[Fact]
		public void CanCreateQuery()
		{
			var query = new Query<User>();

			Assert.Equal(typeof(User), query.EntityType);
			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.Empty, query.Parameters.QueryType);
			Assert.Null(query.Parameters.Expression);
		}

		[Fact]
		public void CanApplyWhereClause()
		{
			Expression<Func<User, bool>> expression = x => x.Person != null;
			var rootQuery = new Query<User>();
			var query = (Query<User>)(rootQuery.Where(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.Where, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyWhereClauseWithContext()
		{
			Expression<Func<User, IQueryContext, bool>> expression = (x, c) => x.Person != null;
			var rootQuery = new Query<User>();
			var query = (Query<User>)(rootQuery.Where(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.Where, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyProjectionClause()
		{
			Expression<Func<User, string>> expression = x => x.ToString();
			var rootQuery = new Query<User>();
			var query = (Query<string>)(rootQuery.Projection(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(string), query.Parameters.OutType);
			Assert.Equal(QueryType.Projection, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyOrderByClause()
		{
			Expression<Func<User, string>> expression = x => x.ToString();
			var rootQuery = new Query<User>();
			var query = (Query<User>)(rootQuery.OrderBy(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.OrderBy, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyOrderByDescendingClause()
		{
			Expression<Func<User, string>> expression = x => x.ToString();
			var rootQuery = new Query<User>();
			var query = (Query<User>)(rootQuery.OrderByDescending(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.OrderByDescending, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplySkipClause()
		{
			var rootQuery = new Query<User>();
			var query = (Query<User>)(rootQuery.Skip(10));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.Skip, query.Parameters.QueryType);
			Assert.Null(query.Parameters.Expression);
			Assert.Equal(10, ((SkipQueryParameter)query.Parameters).Count);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplySkipWhileClause()
		{
			Expression<Func<User, bool>> expression = x => x.Person != null;
			var rootQuery = new Query<User>();
			var query = (Query<User>)(rootQuery.SkipWhile(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.SkipWhile, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyTakeClause()
		{
			var rootQuery = new Query<User>();
			var query = (Query<User>)(rootQuery.Take(10));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.Take, query.Parameters.QueryType);
			Assert.Null(query.Parameters.Expression);
			Assert.Equal(10, ((TakeQueryParameter)query.Parameters).Count);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyTakeWhileClause()
		{
			Expression<Func<User, bool>> expression = x => x.Person != null;
			var rootQuery = new Query<User>();
			var query = (Query<User>)(rootQuery.TakeWhile(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.TakeWhile, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyGroupByClause()
		{
			Expression<Func<User, string>> expression = x => x.ToString();
			var rootQuery = new Query<User>();
			var query = (Query<IGrouping<string, User>>)(rootQuery.GroupBy(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(IGrouping<string, User>), query.Parameters.OutType);
			Assert.Equal(QueryType.GroupBy, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);
			Assert.Equal(typeof(string), ((GroupByQueryParameter)query.Parameters).KeyType);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyJoinClause()
		{
			var source = new Query<Person>();
			Expression<Func<User, Person>> outerKeySelectorExpression = x => x.Person;
			Expression<Func<Person, Person>> innerKeySelectorExpression = x => x;
			Expression<Func<User, Person, string>> resultSelectedExpression = (o, i) => o.ToString() + i.ToString();
			var rootQuery = new Query<User>();
			var query = (Query<string>)(rootQuery.Join(source, outerKeySelectorExpression, innerKeySelectorExpression, resultSelectedExpression));

			Assert.Null(query.Parameters.InType);
			Assert.Equal(typeof(string), query.Parameters.OutType);
			Assert.Equal(QueryType.Join, query.Parameters.QueryType);
			Assert.Equal(source, ((JoinQueryParameter)query.Parameters).InnerSource);
			Assert.Equal(typeof(Person), ((JoinQueryParameter)query.Parameters).InnerType);
			Assert.Equal(typeof(User), ((JoinQueryParameter)query.Parameters).OuterType);
			Assert.Equal(typeof(Person), ((JoinQueryParameter)query.Parameters).KeyType);
			Assert.Equal(innerKeySelectorExpression, ((JoinQueryParameter)query.Parameters).InnerKeySelectorExpression);
			Assert.Equal(outerKeySelectorExpression, ((JoinQueryParameter)query.Parameters).OuterKeySelectorExpression);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyThenByClause()
		{
			Expression<Func<User, int>> expression = x => x.Orders.Count;
			var rootQuery = new Query<User>().OrderBy(x => x.Person);
			var query = (Query<User>)(rootQuery.ThenBy(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.ThenBy, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);
			Assert.Equal(typeof(int), ((ThenByQueryParameter)query.Parameters).KeyType);

			Assert.Same(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyThenByDescendingClause()
		{
			Expression<Func<User, int>> expression = x => x.Orders.Count;
			var rootQuery = new Query<User>().OrderBy(expression);
			var query = (Query<User>)(rootQuery.ThenByDescending(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.ThenByDescending, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);
			Assert.Equal(QueryType.ThenByDescending, query.Parameters.QueryType);
			Assert.Equal(typeof(int), ((ThenByDescendingQueryParameter)query.Parameters).KeyType);

			Assert.Same(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyFetchClause()
		{
			Expression<Func<User, Person>> expression = x => x.Person;
			var rootQuery = new Query<User>();
			var query = (Query<User>)(rootQuery.Fetch(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.Fetch, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);
			Assert.Equal(typeof(Person), ((FetchParameter)query.Parameters).FetchType);

			Assert.Equal(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyThenFetchClause()
		{
			Expression<Func<Person, byte[]>> expression = x => x.Photo;
			var rootQuery = new Query<User>().Fetch(x => x.Person);
			var query = (FetchedQuery<User, byte[]>)(rootQuery.ThenFetch(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.ThenFetch, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);
			Assert.Equal(typeof(byte[]), ((ThenFetchParameter)query.Parameters).FetchType);
			Assert.Equal(typeof(Person), ((ThenFetchParameter)query.Parameters).ParentType);
			Assert.Equal(typeof(User), ((ThenFetchParameter)query.Parameters).RootType);
			Assert.False(((ThenFetchParameter)query.Parameters).ParentIsCollection);

			Assert.Same(rootQuery, query.Parent);
		}

		[Fact]
		public void CanApplyThenFetchClauseForCollection()
		{
			Expression<Func<Order, ICollection<Good>>> expression = x => x.Goods;
			var rootQuery = new Query<User>().Fetch(x => x.Orders);
			var query = (FetchedQuery<User, ICollection<Good>>)(rootQuery.ThenFetch(expression));

			Assert.Equal(typeof(User), query.Parameters.InType);
			Assert.Equal(typeof(User), query.Parameters.OutType);
			Assert.Equal(QueryType.ThenFetch, query.Parameters.QueryType);
			Assert.Equal(expression, query.Parameters.Expression);
			Assert.Equal(typeof(ICollection<Good>), ((ThenFetchParameter)query.Parameters).FetchType);
			Assert.Equal(typeof(Order), ((ThenFetchParameter)query.Parameters).ParentType);
			Assert.Equal(typeof(User), ((ThenFetchParameter)query.Parameters).RootType);
			Assert.True(((ThenFetchParameter)query.Parameters).ParentIsCollection);

			Assert.Same(rootQuery, query.Parent);
		}
	}
}
