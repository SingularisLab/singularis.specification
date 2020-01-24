using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Singularis.Specification.Executor.Nhibernate.Spcifications
{
	class WithFetchCollection : Specification<User>
	{
		public Expression<Func<User, ICollection<Order>>> Expression = x => x.Orders;

		public WithFetchCollection()
		{
			Query = Source().Fetch(Expression);
		}
	}
}