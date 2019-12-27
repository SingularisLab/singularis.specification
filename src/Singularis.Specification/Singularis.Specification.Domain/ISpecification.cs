using System;
using Singularis.Specification.Domain.Query;

namespace Singularis.Specification.Domain
{
	public interface ISpecification
	{
		IQuery Query { get; }
		IQueryContext Context { get; }
		Type ResultType { get; }
	}

	public interface ISpecification<T> : ISpecification
	{
	}
}