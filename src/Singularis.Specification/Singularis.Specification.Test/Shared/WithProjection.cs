using System;
using System.Linq.Expressions;
using Singularis.Specification.Definition;
using Singularis.Specification.Test.Singularis.Specification.Definition.Models;

namespace Singularis.Specification.Test.Shared
{
	class WithProjection : Specification<User>
	{
		public Expression<Func<User, int>> Expression = x => x.Id;

		public WithProjection()
		{
			Query = Source().Projection(Expression);
		}
	}
}