using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Shared
{
	class WithSkipWhile : Specification<User>
	{
		public Expression<Func<User, bool>> Expression = x => x.Person != null;

		public WithSkipWhile()
		{
			Query = Source().SkipWhile(Expression);
		}
	}
}