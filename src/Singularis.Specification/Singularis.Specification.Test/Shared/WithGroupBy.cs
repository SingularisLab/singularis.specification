using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Shared
{
	class WithGroupBy : Specification<User>
	{
		public Expression<Func<User, Person>> Expression = x => x.Person;

		public WithGroupBy()
		{
			Query = Source().GroupBy(Expression);
		}
	}
}