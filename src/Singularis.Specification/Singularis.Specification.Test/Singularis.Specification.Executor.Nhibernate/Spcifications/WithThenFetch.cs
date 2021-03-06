﻿using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Singularis.Specification.Executor.Nhibernate.Spcifications
{
	class WithThenFetch : Specification<User>
	{
		public Expression<Func<Order, Address>> Expression = x => x.Address;

		public WithThenFetch()
		{
			Query = Source().Fetch(x => x.Orders).ThenFetch(Expression);
		}
	}
}