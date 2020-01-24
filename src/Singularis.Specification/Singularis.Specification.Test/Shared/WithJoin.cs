using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Shared
{
	class WithJoin : Specification<User>
	{
		public class Result
		{
			public int Id1;
			public int Id2;
		}

		public Expression<Func<User, int>> OuterKeySelector = x => x.Id;
		public Expression<Func<User, int>> InnerKeySelector = x => x.Id;
		public Expression<Func<User, User, Result>> ResultSelector = (a, b) => new Result { Id1 = a.Id, Id2 = b.Id };
		public IQuery<User> SubQuery { get; }

		public WithJoin()
		{
			SubQuery = Source<User>();
			Query = Source().Join(
				SubQuery,
				OuterKeySelector,
				InnerKeySelector,
				ResultSelector);
		}
	}
}