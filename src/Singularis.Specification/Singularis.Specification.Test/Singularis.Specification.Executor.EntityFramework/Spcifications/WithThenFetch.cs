using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Query;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Singularis.Specification.Executor.EntityFramework.Spcifications
{
	class WithThenFetch : Specification<User>
	{
		public Expression<Func<Person, byte[]>> Expression = x => x.Photo;

		public WithThenFetch()
		{
			Query = Source().Fetch(x => x.Person).ThenFetch(Expression);
		}
	}
}