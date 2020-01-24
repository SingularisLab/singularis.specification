using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Singularis.Specification.Executor.Nhibernate.Spcifications
{
	class WithThenFetchCollection : Specification<User>
	{
		public Expression<Func<Order, ICollection<Good>>> Expression = x => x.Goods;

		public WithThenFetchCollection()
		{
			Query = Source().Fetch(x => x.Orders).ThenFetch(Expression);
		}
	}
}
