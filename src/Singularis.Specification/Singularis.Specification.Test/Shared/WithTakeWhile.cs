using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Shared
{
	class WithTakeWhile : Specification<User>
	{
		public Expression<Func<User, bool>> Expression = x => x.Person != null;

		public WithTakeWhile()
		{
			Query = Source().TakeWhile(Expression);
		}
	}
}