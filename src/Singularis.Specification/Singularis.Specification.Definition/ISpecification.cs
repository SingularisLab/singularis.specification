using System;
using Singularis.Specification.Definition.Query;

namespace Singularis.Specification.Definition
{
	public interface ISpecification<T> : ISpecification
	{
	}

	public interface ISpecification
	{
		IQuery Query { get; }
		Type ResultType { get; }
	}
}