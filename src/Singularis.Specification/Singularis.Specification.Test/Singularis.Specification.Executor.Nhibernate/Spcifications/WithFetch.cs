using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Singularis.Specification.Executor.Nhibernate.Spcifications
{
	class WithFetch : Specification<User>
	{
		public Expression<Func<User, Person>> Expression = x => x.Person;

		public WithFetch()
		{
			Query = Source().Fetch(Expression);
		}
	}
}