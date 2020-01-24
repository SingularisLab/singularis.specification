using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using NHibernate;
using NHibernate.Linq;
using Singularis.Specification.Definition;
using Singularis.Specification.Definition.Services;
using Singularis.Specification.Executor.Common;
using ReflectionHelper = Singularis.Specification.Executor.Common.ReflectionHelper;

namespace Singularis.Specification.Executor.Nhibernate
{
	public class Repository
	{
		protected static readonly MethodInfo ToListMethodInfo;
		protected static readonly MethodInfo FirstOrDefaultMethodInfo;
		protected static readonly MethodInfo CountMethodInfo;
		protected static readonly MethodInfo DeleteMethodInfo;

		static Repository()
		{
			DeleteMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(DmlExtensionMethods),
				nameof(DmlExtensionMethods.DeleteAsync),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(CancellationToken), 1));

			ToListMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(LinqExtensionMethods),
				nameof(LinqExtensionMethods.ToListAsync),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(CancellationToken), 1));

			FirstOrDefaultMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(LinqExtensionMethods),
				nameof(LinqExtensionMethods.FirstOrDefaultAsync),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(CancellationToken), 1));

			CountMethodInfo = ReflectionHelper.FindMethod(
				BindingFlags.Static | BindingFlags.Public,
				typeof(LinqExtensionMethods),
				nameof(LinqExtensionMethods.CountAsync),
				null,
				new ArgumentConstraint(typeof(IQueryable<>), 1),
				new ArgumentConstraint(typeof(CancellationToken), 1));
		}
	}

	public class Repository<TId> : Repository, IRepository<TId>
	{
		private readonly ISession _session;
		private readonly RepositorySettings _settings;
		private readonly SpecificationExecutor _specificationExecutor;

		public Repository(
			ISession session,
			RepositorySettings settings)
		{
			_session = session;
			_settings = settings;
			_specificationExecutor = new SpecificationExecutor();
		}

		public Task<object> GetAsync(TId id, Type entityType, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _session.GetAsync(entityType, id, cancellationToken);
		}

		public async Task<T> GetAsync<T>(TId id, CancellationToken cancellationToken = default(CancellationToken))
			where T : class
		{
			var itm = await GetAsync(id, typeof(T), cancellationToken);
			return (T)itm;
		}

		public async Task<object> GetAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = _specificationExecutor.ExecuteSpecification(_session, specification);
			var firstOrDefault = FirstOrDefaultMethodInfo.MakeGenericMethod(specification.ResultType);
			dynamic task = firstOrDefault.Invoke(query, new object[] { query, cancellationToken });

			await task;

			return task.Result;
		}

		public async Task<T> GetAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default(CancellationToken))
			where T : class
		{
			var item = await GetAsync((ISpecification)specification, cancellationToken);
			return (T)item;
		}

		public async Task<T> GetAsync<T>(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
			where T : class
		{
			var item = await GetAsync(specification, cancellationToken);
			return (T)item;
		}

		public async Task<ICollection> ListAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = _specificationExecutor.ExecuteSpecification(_session, specification);
			var toList = ToListMethodInfo.MakeGenericMethod(specification.ResultType);
			dynamic task = toList.Invoke(query, new object[] { query, cancellationToken });

			await task;

			return task.Result;
		}

		public Task<IReadOnlyCollection<T>> ListAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken = default(CancellationToken))
			where T : class
		{
			return ListAsync<T>((ISpecification)specification, cancellationToken);
		}

		public async Task<IReadOnlyCollection<T>> ListAsync<T>(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
			where T : class
		{
			var items = await _specificationExecutor
				.ExecuteSpecification<T>(_session, specification)
				.ToListAsync(cancellationToken);

			return items.AsReadOnly();
		}

		public Task<int> CountAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = _specificationExecutor.ExecuteSpecification(_session, specification);
			var countMethod = CountMethodInfo.MakeGenericMethod(specification.ResultType);
			var task = (Task<int>)countMethod.Invoke(query, new object[] { query, cancellationToken });

			return task;
		}

		public async Task SaveAsync(object entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var scope = new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = _settings.TransactionTimeout,
					IsolationLevel = _settings.TransactionIsolationLevel
				},
				TransactionScopeAsyncFlowOption.Enabled))
			{
				await _session.SaveOrUpdateAsync(entity, cancellationToken);
				await _session.FlushAsync(cancellationToken);
				scope.Complete();
			}
		}

		public async Task SaveAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var scope = new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = _settings.TransactionTimeout,
					IsolationLevel = _settings.TransactionIsolationLevel
				},
				TransactionScopeAsyncFlowOption.Enabled))
			{
				foreach (var entity in entities)
					await _session.SaveOrUpdateAsync(entity, cancellationToken);
				await _session.FlushAsync(cancellationToken);

				scope.Complete();
			}
		}

		public async Task DeleteAsync(object entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var scope = new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = _settings.TransactionTimeout,
					IsolationLevel = _settings.TransactionIsolationLevel
				},
				TransactionScopeAsyncFlowOption.Enabled))
			{
				await _session.DeleteAsync(entity, cancellationToken);
				await _session.FlushAsync(cancellationToken);

				scope.Complete();
			}
		}

		public async Task DeleteAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var scope = new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = _settings.TransactionTimeout,
					IsolationLevel = _settings.TransactionIsolationLevel
				},
				TransactionScopeAsyncFlowOption.Enabled))
			{
				foreach (var entity in entities)
					await _session.DeleteAsync(entity, cancellationToken);
				await _session.FlushAsync(cancellationToken);

				scope.Complete();
			}
		}

		public async Task DeleteAsync(ISpecification specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var scope = new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					Timeout = _settings.TransactionTimeout,
					IsolationLevel = _settings.TransactionIsolationLevel
				},
				TransactionScopeAsyncFlowOption.Enabled))
			{
				var query = _specificationExecutor.ExecuteSpecification(_session, specification);
				var delete = DeleteMethodInfo.MakeGenericMethod(specification.ResultType);
				var task = delete.Invoke(query, new object[] { query, cancellationToken });

				await (Task)task;

				scope.Complete();
			}
		}
	}
}