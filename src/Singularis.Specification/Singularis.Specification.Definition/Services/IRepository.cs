using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Singularis.Specification.Definition.Services
{
	public interface IRepository<in TId>
	{
		Task<T> GetAsync<T>(TId id, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
		Task<object> GetAsync(TId id, Type entityType, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> GetAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
		Task<T> GetAsync<T>(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
		Task<object> GetAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken));

		Task<ICollection> ListAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken));
		Task<IReadOnlyCollection<T>> ListAsync<T>(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
		Task<IReadOnlyCollection<T>> ListAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default(CancellationToken)) where T : class;

		Task<int> CountAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken));

		Task SaveAsync(object entity, CancellationToken cancellationToken = default(CancellationToken));
		Task SaveAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken));

		Task DeleteAsync(object entity, CancellationToken cancellationToken = default(CancellationToken));
		Task DeleteAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken));
		Task DeleteAsync(ISpecification spefication, CancellationToken cancellationToken = default(CancellationToken));
	}
}
