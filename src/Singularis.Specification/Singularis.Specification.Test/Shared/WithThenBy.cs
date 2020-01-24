using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Shared
{
	class WithThenBy : Specification<User>
	{
		public Expression<Func<User, int>> Expression = x => x.Id;

		public WithThenBy()
		{
			Query = Source().OrderByDescending(x => x.Id).ThenBy(Expression);
		}
	}
}