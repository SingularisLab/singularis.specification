using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;

namespace Singularis.Specification.Test.Singularis.Specification.Executor.EntityFramework.Fake
{
	public class FakeSet<T> : DbSet<T>, IQueryable<T>, IQueryable
		where T : class
	{
		private readonly IQueryable<T> _inner;
		private EntityQueryProvider _pr;

		public FakeSet()
		{
			var a = new Mock<IQueryCompiler>();
			_pr = new EntityQueryProvider(a.Object);
			_inner = new List<T>().AsQueryable();
		}

		public IQueryProvider Provider => _pr;

		public Expression Expression => _inner.Expression;

		public Type ElementType => _inner.ElementType;
	}
}